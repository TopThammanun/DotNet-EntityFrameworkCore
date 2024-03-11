using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet_EntityFrameworkCore.Core
{
    public class DeleteRecordFailedException : Exception
    {
        public DeleteRecordFailedException(string message, Exception innerException)
            : base(message, innerException)
        {

        }

        public DeleteRecordFailedException(Exception innerException)
            : base("Deleting Failed", innerException)
        {

        }
    }
}
