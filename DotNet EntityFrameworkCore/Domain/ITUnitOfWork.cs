using System.Threading.Tasks;
using DotNet_EntityFrameworkCore.DataCore;
using DotNet_EntityFrameworkCore.Domain;
using DotNet_EntityFrameworkCore.Domain.Repository;

namespace HealthTeam.PCU.Microservice.Domain
{
    public interface ITUnitOfWork : IUnitOfWork
    {
        Task<int> SaveWithoutTracking();
        IMstPlanRepository MstPlanRepository { get; }
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

        public IMstPlanRepository MstPlanRepository { get; private set; }

        protected override void InitialRepositories(IDBContext dbContext)
        {
            MstPlanRepository = new MstPlanRepository(dbContext);
        }
}
}