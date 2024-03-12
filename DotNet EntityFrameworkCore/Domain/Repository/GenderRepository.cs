using DotNet_EntityFrameworkCore.DataCore;
using DotNet_EntityFrameworkCore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthTeam.PCU.Microservice.Domain
{
    public interface IGenderRepository : IRepository<PCU_MST_GENDER>
    {

    }
    public class GenderRepository : Repository<PCU_MST_GENDER>, IGenderRepository
    {
        public GenderRepository(IDBContext dBContext)
            : base(dBContext)
        {

        }
    }
}
