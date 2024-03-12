using DotNet_EntityFrameworkCore.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet_EntityFrameworkCore.Domain.Entities
{
    public class PCU_MST_GENDER : EntityBase
    {
        public int GEND_ID { get; set; }
        public string? GEND_CODE { get; set; }
        public string? GEND_NAME_TH { get; set; }
        public string? GEND_NAME_EN { get; set; }
        public string? IS_ACTIVE { get; set; }
    }
}
