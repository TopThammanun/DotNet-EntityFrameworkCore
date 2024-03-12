using System.Threading.Tasks;
using DotNet_EntityFrameworkCore.DataCore;
using DotNet_EntityFrameworkCore.Domain;
using HealthTeam.PCU.Microservice.Domain;

namespace DotNet_EntityFrameworkCore.Domain
{
    public interface ITUnitOfWork : IUnitOfWork
    {
        Task<int> SaveWithoutTracking();
        IGenderRepository GenderRepository { get; }
    }

    public class TUnitOfWork : UnitOfWork, ITUnitOfWork
    {
        public TUnitOfWork(ITDBContext dBContext)
            : base(dBContext)
        {
            this.dBContext = dBContext;
        }

        private ITDBContext dBContext;

        public async Task<int> SaveWithoutTracking()
        {
            return await this.dBContext.SaveWithoutChangeModDataAsync();
        }

        public IGenderRepository GenderRepository { get; private set; }

        protected override void InitialRepositories(IDBContext dbContext)
        {
            GenderRepository = new GenderRepository(dbContext);
        }
}
}