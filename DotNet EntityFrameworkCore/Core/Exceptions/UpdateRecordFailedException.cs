using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet_EntityFrameworkCore.Core
{
    public class UpdateRecordFailedException : Exception
    {
        public UpdateRecordFailedException(string message, Exception innerException)
            : base(message, innerException)
        {

        }

        public UpdateRecordFailedException(Exception innerException)
            : base("Update Failed", innerException)
        {

        }
    }
}
