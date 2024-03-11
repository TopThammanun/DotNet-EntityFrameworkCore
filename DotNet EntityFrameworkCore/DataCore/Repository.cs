using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Linq.Expressions;

namespace DotNet_EntityFrameworkCore.DataCore
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IDBContext DBContext { get; }
        IQueryable<TEntity> GetAll();
        void Add(TEntity entity);
        void AddRange(List<TEntity> entity);
        void UpdateRange(List<TEntity> entity);
        void Remove(TEntity entity);
        TEntity Get<TKey>(TKey key);
        Task<TEntity> GetAsync<TKey>(TKey key);
        IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
        bool IsExist<TKey>(TKey key);
        void InsertBulk(IEnumerable<TEntity> entities);
        DatabaseFacade Database { get; }
        void Update(TEntity entity);
        bool CompareEntity(TEntity entity);
    }

    public abstract class Repository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        public IDBContext DBContext { get => dbContext; }
        private IDBContext dbContext;
        public Repository(IDBContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public IQueryable<TEntity> GetAll()
        {
            return DBContext.Set<TEntity>();
        }
        public void Add(TEntity entity)
        {
            DBContext.Set<TEntity>().Add(entity);
        }
        public void AddRange(List<TEntity> entity)
        {
            DBContext.Set<TEntity>().AddRange(entity);
        }
        public async Task AddAsync(TEntity entity)
        {
            await DBContext.Set<TEntity>().AddAsync(entity);
        }
        public void Remove(TEntity entity)
        {
            DBContext.Remove(entity);
            //DBContext.Set<TEntity>().Remove(entity);
        }
        public TEntity Get<TKey>(TKey key)
        {
            return DBContext.GetEntity<TEntity, TKey>(key);
        }

        public async Task<TEntity> GetAsync<TKey>(TKey key)
        {
            return await DBContext.GetEntityAsync<TEntity, TKey>(key);
        }

        public IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return DBContext.Set<TEntity>().Where(predicate);
        }

        public bool IsExist<TKey>(TKey key)
        {
            return DBContext.IsExist<TEntity, TKey>(key);
        }

        public void InsertBulk(IEnumerable<TEntity> entities)
        {
            DBContext.InsertBulk(entities);
        }
        public void Update(TEntity entity)
        {
            DBContext.Set<TEntity>().Update(entity);
        }
        public void UpdateRange(List<TEntity> entity)
        {
            DBContext.Set<TEntity>().UpdateRange(entity);
        }

        public bool CompareEntity(TEntity entity)
        {
            return DBContext.CompareEntity(entity);
        }

        public DatabaseFacade Database { get => dbContext.Database; }
    }
}
