using DotNet_EntityFrameworkCore.DataCore;
using DotNet_EntityFrameworkCore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthTeam.PCU.Microservice.Domain
{
    public class PCU_MST_GENDEREntityTypeConfiguration : EntityTypeConfiguration<PCU_MST_GENDER>
    {
        public PCU_MST_GENDEREntityTypeConfiguration(ModelBuilder modelBuilder)
            : base(modelBuilder)
        {
            builder.HasKey(t => t.GEND_ID).HasName("PRIMARY");
            builder.ToTable("pcu_mst_gender");
        }
    }
}
