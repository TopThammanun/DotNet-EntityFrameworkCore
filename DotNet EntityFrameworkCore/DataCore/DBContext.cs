using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Storage;

namespace DotNet_EntityFrameworkCore.DataCore
{
    public interface IDBContext : IDisposable
    {
        DatabaseFacade Database { get; }
        EntityEntry<TEntity> Add<TEntity>(TEntity entity) where TEntity : class;
        Task<EntityEntry<TEntity>> AddAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default) where TEntity : class;
        void AddRange(IEnumerable<object> entities);
        Task AddRangeAsync(IEnumerable<object> entities, CancellationToken cancellationToken = default);
        EntityEntry Remove(object entity);
        EntityEntry<TEntity> Remove<TEntity>([NotNull] TEntity entity) where TEntity : class;
        void RemoveRange(IEnumerable<object> entities);
        IEnumerable<T> SqlQuery<T>(string sql, params object[] parameters) where T : class;
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        int ExecuteSqlRaw(string sql, params object[] parameters);
        int SaveChanges();
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        TEntity GetEntity<TEntity, TKey>(TKey key) where TEntity : class;
        Task<TEntity> GetEntityAsync<TEntity, TKey>(TKey key) where TEntity : class;
        bool IsExist<TEntity, TKey>(TKey key) where TEntity : class;
        Task<bool> IsExistAsync<TEntity, TKey>(TKey key) where TEntity : class;
        void InsertBulk<T>(IEnumerable<T> entities) where T : class;
        DataTable ConvertToEntitiesDataTable<T>(IEnumerable<T> entities) where T : class;
        EntityEntry<TEntity> Update<TEntity>(TEntity entity) where TEntity : class;
        bool CompareEntity<TEntity>(TEntity entity) where TEntity : class;
        bool CompareEntity<TEntity>(TEntity entity, Expression<Func<TEntity, bool>> search) where TEntity : class;
    }
    public abstract class DBContext : DbContext, IDBContext
    {
        private IDbModelConfigure configure;
        public DBContext(IDbModelConfigure configure)
        {
            this.configure = configure;
        }
        public virtual IEnumerable<T> SqlQuery<T>(string sql, params object[] parameters)
           where T : class
        {
            IEnumerable<T> result;
            using (var tempContext = new TempContext<T>(configure))
            {
                result = tempContext.Set<T>().FromSqlRaw(sql, parameters).ToList();
                return result;
            }
        }

        public virtual int ExecuteSqlRaw(string sql, params object[] parameters)
        {
            return Database.ExecuteSqlRaw(sql, parameters);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            configure.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            configure.OnModelCreating(modelBuilder);
        }
        public virtual async Task<EntityEntry<TEntity>> AddAsync<TEntity>(TEntity entity, CancellationToken cancellationToken)
            where TEntity : class
        {
            return await base.AddAsync(entity, cancellationToken);
        }

        public void InsertBulk<T>(IEnumerable<T> entities) where T : class
        {
            var dt = ConvertToEntitiesDataTable(entities);
            var tableName = dt.TableName;
            var connection = (SqlConnection)Database.GetDbConnection();
            using (var bulk = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, Database.CurrentTransaction == null ? null : (SqlTransaction)Database.CurrentTransaction.GetDbTransaction()))
            {
                dt.Columns.Cast<DataColumn>().ToList().ForEach(x =>
                {
                    bulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping(x.ColumnName, x.ColumnName));
                });
                bulk.DestinationTableName = tableName;

                bulk.BulkCopyTimeout = 60;
                bulk.BatchSize = 5000;
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                bulk.WriteToServer(dt);
                connection.Close();
            }
        }

        public DataTable ConvertToEntitiesDataTable<T>(IEnumerable<T> entities) where T : class
        {
            var entity = Model.FindEntityType(typeof(T));
            var schema = entity.GetSchema();
            var tableName = entity.GetTableName();
            var storeObjectIdentifier = StoreObjectIdentifier.Table(tableName, schema);

            if (entity == null)
                throw new Exception("Invalid entity.");
            var props = entity.GetProperties().ToList();
            //Dictionary<ScalarPropertyMapping, PropertyInfo> columnMapping = GetEntityColumnMaping(typeof(T));
            if (props.Count == 0)
                throw new Exception("Invalid entity.");
            var result = new DataTable(tableName);
            foreach (var col in props)
            {
                var colName = col.GetColumnName(storeObjectIdentifier);
                var propInfo = col.PropertyInfo;
                Type proptype;
                if (Nullable.GetUnderlyingType(propInfo.PropertyType) == null)
                    proptype = propInfo.PropertyType;
                else
                    proptype = Nullable.GetUnderlyingType(propInfo.PropertyType);
                if (proptype == typeof(decimal))
                    proptype = typeof(double);
                result.Columns.Add(new DataColumn(colName, proptype) { AllowDBNull = col.IsNullable });
            }
            foreach (var item in entities.ToList())
            {
                var dr = result.NewRow();
                foreach (var col in props)
                {

                    var colName = col.GetColumnName(storeObjectIdentifier);

                    var propInfo = col.PropertyInfo;
                    var value = propInfo.GetValue(item, null);
                    //var value1 = value == null ? null : (Convert.ChangeType(value, Nullable.GetUnderlyingType(propInfo.PropertyType) ?? propInfo.PropertyType));
                    dr[colName] = value != null ? value : DBNull.Value;
                }
                result.Rows.Add(dr);
            }
            return result;
        }
        public TEntity GetEntity<TEntity, TKey>(TKey key) where TEntity : class
        {
            return Set<TEntity>().Where(CreateFilterByKeyExpression<TEntity, TKey>(key)).FirstOrDefault();
        }

        public async Task<TEntity> GetEntityAsync<TEntity, TKey>(TKey key) where TEntity : class
        {
            return await Set<TEntity>().Where(CreateFilterByKeyExpression<TEntity, TKey>(key)).FirstOrDefaultAsync();
        }

        public bool IsExist<TEntity, TKey>(TKey key) where TEntity : class
        {
            return Set<TEntity>().Where(CreateFilterByKeyExpression<TEntity, TKey>(key)).Any();
        }
        public async Task<bool> IsExistAsync<TEntity, TKey>(TKey key) where TEntity : class
        {
            return await Set<TEntity>().Where(CreateFilterByKeyExpression<TEntity, TKey>(key)).AnyAsync();
        }

        public bool CompareEntity<TEntity>(TEntity entity, Expression<Func<TEntity, bool>> search) where TEntity : class
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            if (search == null)
                throw new ArgumentNullException(nameof(search));

            var props = typeof(TEntity).GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => (p.PropertyType.IsValueType || p.PropertyType == typeof(string)) && p.GetCustomAttribute<NotMappedAttribute>() == null).ToList();

            var dbEntity = Set<TEntity>().Where(search).AsNoTracking().FirstOrDefault();
            if (dbEntity == null)
                return false;
            foreach (var prop in props)
            {
                var valL = prop.GetValue(entity);
                var valR = prop.GetValue(dbEntity);

                if (valL == null && valR == null)
                    continue;
                if (valL == null && valR != null)
                    return false;
                else if (!valL.Equals(valR))
                    return false;
            }

            return true;
        }

        public bool CompareEntity<TEntity>(TEntity entity) where TEntity : class
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var entityType = typeof(TEntity);
            var props = typeof(TEntity).GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => (p.PropertyType.IsValueType || p.PropertyType == typeof(string)) && p.GetCustomAttribute<NotMappedAttribute>() == null).ToList();

            var entitytype1 = Model.FindEntityType(entityType);
            var pkProps = entitytype1.FindPrimaryKey().Properties.Select(t => t.PropertyInfo).ToList();
            if (!pkProps.Any())
                throw new InvalidOperationException("Entity type didn't not map primary key.");

            Expression<Func<TEntity, bool>> search = t => false;

            var parm = Expression.Parameter(entityType, "t");
            var keyVal = pkProps[0].GetValue(entity);
            var variable = Expression.Constant(keyVal);
            var exprop = Expression.Property(parm, pkProps[0]);
            var body = Expression.Equal(exprop, variable);

            for (int i = 1; i < pkProps.Count(); i++)
            {
                keyVal = pkProps[i].GetValue(entity);
                variable = Expression.Constant(keyVal);
                exprop = Expression.Property(parm, pkProps[i]);
                var eqEx = Expression.Equal(exprop, variable);
                body = Expression.AndAlso(body, eqEx);
            }

            search = Expression.Lambda<Func<TEntity, bool>>(body, parm);

            var dbEntity = Set<TEntity>().Where(search).AsNoTracking().FirstOrDefault();
            if (dbEntity == null)
                return false;
            foreach (var prop in props)
            {
                var valL = prop.GetValue(entity);
                var valR = prop.GetValue(dbEntity);

                if (valL == null && valR == null)
                    continue;
                if (valL == null && valR != null)
                    return false;
                else if (!valL.Equals(valR))
                    return false;
            }

            return true;
        }

        private Expression<Func<TEntity, bool>> CreateFilterByKeyExpression<TEntity, TKey>(TKey key)
        {
            var entityType = typeof(TEntity);

            var entity = Model.FindEntityType(entityType);
            var pkPropNames = entity.FindPrimaryKey().Properties.Select(t => t.Name).ToList();

            if (typeof(TKey).IsValueType || typeof(TKey) == typeof(string))
            {
                //var prop = entityType.GetProperty(entityMetadata.KeyProperties[0].Name);
                if (pkPropNames.Count() > 1)
                    throw new InvalidOperationException("Invalid key argument, Type of key dosen't match.");
                var parm = Expression.Parameter(entityType, "t");
                var variable = Expression.Constant(key);
                var exprop = Expression.Property(parm, pkPropNames[0]);
                var body = Expression.Equal(exprop, variable);
                var final = Expression.Lambda<Func<TEntity, bool>>(body, parm);
                return final;
            }
            else
            {
                var inputKeyProps = key.GetType().GetProperties();
                var parm = Expression.Parameter(entityType, "t");
                var exprop = Expression.Property(parm, pkPropNames[0]);
                var prop = inputKeyProps.Where(t => t.Name == pkPropNames[0]).FirstOrDefault();
                if (prop == null)
                    throw new InvalidOperationException("Invalid key argument, Key member dosen't match.");
                var val = prop.GetValue(key);
                var variable = Expression.Constant(val);
                var body = Expression.Equal(exprop, variable);
                for (int i = 1; i < pkPropNames.Count(); i++)
                {
                    prop = inputKeyProps.Where(t => t.Name == pkPropNames[i]).FirstOrDefault();
                    if (prop == null)
                        throw new InvalidOperationException("Invalid key argument, Key member dosen't match.");
                    val = prop.GetValue(key);
                    variable = Expression.Constant(val);
                    body = Expression.And(body, Expression.Equal(exprop, variable));
                }
                var final = Expression.Lambda<Func<TEntity, bool>>(body, parm);
                return final;
            }
        }

    }

    public class SqlResult<T>
    {
        [Column("Value")]
        public T Value { get; set; }
    }
    class TempContext<TResult> : DbContext
       where TResult : class
    {
        private IDbModelConfigure configure;

        public TempContext(IDbModelConfigure configure)
           : base()
        {
            this.configure = configure;
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<TResult>().HasNoKey();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            configure.OnConfiguring(optionsBuilder);
        }
        public IEnumerable<TResult> SqlQuery(string sql, params object[] parameters)
        {
            return Set<TResult>().FromSqlRaw(sql, parameters);
        }
    }
}
