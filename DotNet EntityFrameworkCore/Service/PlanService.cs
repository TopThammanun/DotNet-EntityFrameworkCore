using DotNet_EntityFrameworkCore.Domain;
using Microsoft.EntityFrameworkCore;

namespace DotNet_EntityFrameworkCore.Service
{
    public interface IPlanService
    {
        Task<string> Test();
    }
    public class PlanService : IPlanService
    {
        TUnitOfWork unitOfWork;
        public PlanService(TUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<string> Test()
        {
            var result = await unitOfWork.GenderRepository.Find(t => t.GEND_ID == 4).FirstOrDefaultAsync();
            var x = result.GEND_NAME_TH;
            if (!String.IsNullOrEmpty(x))
            {
                return x;
            }
            return "Error";
        }
    }
}
