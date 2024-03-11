using DotNet_EntityFrameworkCore.Core;
using DotNet_EntityFrameworkCore.Domain.EntityTypeConfigurations;
using Microsoft.EntityFrameworkCore;


namespace DotNet_EntityFrameworkCore.Domain
{
    public class DBConfigure : DbContext
    {
        public void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connetionString = AppSettings.Get<string>("ConnectionStrings:DB");
            var serverVersion = new MariaDbServerVersion(new Version(10, 3, 31));
            optionsBuilder.UseMySql(connetionString, serverVersion);
        }

        public void OnModelCreating(ModelBuilder modelBuilder)
        {
            new MST_PLANEntityTypeConfiguration(modelBuilder);
        }

    }
}
 