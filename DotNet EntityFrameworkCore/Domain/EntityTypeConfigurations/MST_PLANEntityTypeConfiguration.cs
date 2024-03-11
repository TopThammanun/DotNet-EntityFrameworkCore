using DotNet_EntityFrameworkCore.DataCore;
using DotNet_EntityFrameworkCore.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DotNet_EntityFrameworkCore.Domain.EntityTypeConfigurations
{
    public class MST_PLANEntityTypeConfiguration : EntityTypeConfiguration<MST_PLAN>
    {
        public MST_PLANEntityTypeConfiguration(ModelBuilder modelBuilder)
            : base(modelBuilder)
        {
            builder.HasKey(t => t.id_plan);
            builder.ToTable("MST_PLAN");
        }
    }
}
