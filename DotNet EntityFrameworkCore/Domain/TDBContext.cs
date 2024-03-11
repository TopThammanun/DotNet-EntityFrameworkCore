using DotNet_EntityFrameworkCore.Core;
using DotNet_EntityFrameworkCore.DataCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DotNet_EntityFrameworkCore.Domain
{
    public interface ITDBContext : IDBContext
    {
        int SaveWithoutChangeModData();
        Task<int> SaveWithoutChangeModDataAsync(CancellationToken cancellationToken = default);
        EntityEntry<TEntity> RemovePermanent<TEntity>(TEntity entity) where TEntity : class;
    }
    public class TDBContext : DBContext, ITDBContext
    {
        public TDBContext(IDbModelConfigure modelConfigure)
            : base(modelConfigure)
        {

        }

        public int SaveChanges()
        {
            TrackingOnSaveChanges();
            return base.SaveChanges();
        }
        public int SaveWithoutChangeModData()
        {
            return base.SaveChanges();
        }

        public async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            TrackingOnSaveChanges();
            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            TrackingOnSaveChanges();
            return base.SaveChangesAsync(cancellationToken);
        }

        public async Task<int> SaveWithoutChangeModDataAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public async Task<int> SaveWithoutChangeModDataAsync(CancellationToken cancellationToken = default)
        {
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void TrackingOnSaveChanges()
        {
            string userId = SessionData.UserInfo?.Username;

            foreach (EntityEntry item in ChangeTracker.Entries())
            {
                if (item.Entity is EntityBase)
                {
                    IModifyTrackingEntity e = item.Entity as IModifyTrackingEntity;
                    if (item.State == EntityState.Added)
                    {
                        e.CREATED_BY = userId;
                        e.CREATED_DATE = DateTime.Now;
                    }
                    else if (item.State == EntityState.Modified)
                    {
                        e.UPDATED_BY = userId;
                        e.UPDATED_DATE = DateTime.Now;
                    }
                }
            }
        }

        public override EntityEntry<TEntity> Remove<TEntity>(TEntity entity)
        {
            string userId = SessionData.UserInfo?.Username;
            if (entity is IDeleteFlagEntity)
            {
                IDeleteFlagEntity e = entity as IDeleteFlagEntity;
                e.DELETED_BY = userId;
                e.DELETED_DATE = DateTime.Now;
                e.DELETED_FLAG = "Y";
                return this.Entry(entity);
            }
            else
            {
                return base.Remove(entity);
            }
        }

        public EntityEntry<TEntity> RemovePermanent<TEntity>(TEntity entity) where TEntity : class
        {
            return base.Remove(entity);
        }
    }
}
