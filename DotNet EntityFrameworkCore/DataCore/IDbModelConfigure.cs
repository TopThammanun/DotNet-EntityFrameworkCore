using Microsoft.EntityFrameworkCore;

namespace DotNet_EntityFrameworkCore.DataCore
{
    public interface IDbModelConfigure
    {
        void OnModelCreating(ModelBuilder modelBuilder);
        void OnConfiguring(DbContextOptionsBuilder optionsBuilder);
    }
}
