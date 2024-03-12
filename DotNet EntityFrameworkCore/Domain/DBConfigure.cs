using DotNet_EntityFrameworkCore.Core;
using DotNet_EntityFrameworkCore.DataCore;
using HealthTeam.PCU.Microservice.Domain;
using Microsoft.EntityFrameworkCore;


namespace DotNet_EntityFrameworkCore.Domain
{
    public class DBConfigure : IDbModelConfigure
    {
        public void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        /*    string connetionString = AppSettings.Get<string>("ConnectionStrings:DB");*/
            /*var serverVersion = new MariaDbServerVersion(new Version(10, 3, 31));*/
            optionsBuilder.UseMySql("server=localhost;port=3306;user=root;password=123456;database=TEST", Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.10.2-mariadb"));
        }

        public void OnModelCreating(ModelBuilder modelBuilder)
        {
            new PCU_MST_GENDEREntityTypeConfiguration(modelBuilder);
        }
    }
}
 