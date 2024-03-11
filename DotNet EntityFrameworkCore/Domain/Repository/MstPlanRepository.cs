using DotNet_EntityFrameworkCore.DataCore;
using DotNet_EntityFrameworkCore.Domain.Entities;

namespace DotNet_EntityFrameworkCore.Domain.Repository
{
    public interface IMstPlanRepository : IRepository<MST_PLAN>
    {

    }
    public class MstPlanRepository : Repository<MST_PLAN>, IMstPlanRepository
    {
        public MstPlanRepository(IDBContext dBContext)
            : base(dBContext)
        {

        }
    }
}
