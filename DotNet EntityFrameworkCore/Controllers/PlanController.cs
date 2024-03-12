using DotNet_EntityFrameworkCore.Service;
using DotNet_EntityFrameworkCore.WebAPICore;
using Microsoft.AspNetCore.Mvc;

namespace DotNet_EntityFrameworkCore.Controllers
{
    [Route("plan")]
    [ApiController]
    public class PlanController : Controller
    {
        IPlanService planService;
        public PlanController(IPlanService planService)
        {
            this.planService = planService;
        }

        [HttpGet("test")]
        public async Task<IActionResult> GetDepartments()
        {
            var result = await planService.Test();
            return Ok(new APIResultSuccess(result));
        }
    }
}
