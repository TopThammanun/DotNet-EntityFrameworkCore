using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DotNet_EntityFrameworkCore.DataCore
{
    public abstract class EntityTypeConfiguration<TEntity> where TEntity : class
    {
        protected readonly EntityTypeBuilder<TEntity> builder;
        public EntityTypeConfiguration(ModelBuilder modelBuilder)
        {
            builder = modelBuilder.Entity<TEntity>();
        }
    }
}
