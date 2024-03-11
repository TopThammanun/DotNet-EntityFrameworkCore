using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using System.Linq.Expressions;

namespace DotNet_EntityFrameworkCore.DataCore
{
    public interface IUnitOfWork : IDisposable
    {
        DatabaseFacade Database { get; }
        IEnumerable<T> SqlQuery<T>(string sql, params object[] parameters) where T : class;
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        int ExecuteSqlRaw(string sql, params object[] parameters);
        int ExecuteSqlRaw(string sql, CommandType commandType, params object[] parameters);
        int ExecuteSqlRaw(string sql, CommandType commandType, IDbContextTransaction tran, params object[] parameters);
        int SaveChanges();
        bool CompareEntity<TEntity>(TEntity entity) where TEntity : class;
        bool CompareEntity<TEntity>(TEntity entity, Expression<Func<TEntity, bool>> search) where TEntity : class;
    }
    public abstract class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork(IDBContext dbContext)
        {
            this.dbContext = dbContext;
            InitialRepositories(dbContext);
        }
        IDBContext dbContext;

        protected virtual void InitialRepositories(IDBContext dbContext)
        {

        }

        public DatabaseFacade Database => dbContext.Database;
        public IEnumerable<T> SqlQuery<T>(string sql, params object[] parameters) where T : class
        {
            return dbContext.SqlQuery<T>(sql, parameters);
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await dbContext.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            dbContext.Dispose();
        }

        public int ExecuteSqlRaw(string sql, params object[] parameters)
        {
            return dbContext.ExecuteSqlRaw(sql, parameters);
        }

        public int ExecuteSqlRaw(string sql, CommandType commandType, params object[] parameters)
        {
            return ExecuteSqlRaw(sql, commandType, null, parameters);
        }

        public int ExecuteSqlRaw(string sql, CommandType commandType, IDbContextTransaction tran, params object[] parameters)
        {
            var conn = Database.GetDbConnection();
            var currentConnStatus = conn.State;

            var cmd = conn.CreateCommand();
            if (tran != null)
            {
                cmd.Transaction = tran.GetDbTransaction();
            }
            cmd.CommandText = sql;
            cmd.Parameters.AddRange(parameters);
            cmd.CommandType = CommandType.StoredProcedure;

            if (currentConnStatus != ConnectionState.Open)
                Database.OpenConnection();

            int rows = cmd.ExecuteNonQuery();

            if (currentConnStatus == ConnectionState.Closed)
                Database.CloseConnection();

            return rows;

        }

        public int SaveChanges()
        {
            return dbContext.SaveChanges();
        }

        public bool CompareEntity<TEntity>(TEntity entity) where TEntity : class
        {
            return dbContext.CompareEntity(entity);
        }

        public bool CompareEntity<TEntity>(TEntity entity, Expression<Func<TEntity, bool>> search) where TEntity : class
        {
            return dbContext.CompareEntity(entity, search);
        }
    }
}
