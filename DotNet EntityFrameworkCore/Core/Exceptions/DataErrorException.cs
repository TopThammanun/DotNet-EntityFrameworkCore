using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet_EntityFrameworkCore.Core
{
    public class DataErrorException : Exception
    {
        public string ErrorCode { get; }
        public string Detail { get; }
        public DataErrorException(string code, string message)
            : base(message)
        {
            ErrorCode = code;
        }

        public DataErrorException(string code, string message, string detail)
            : base(message)
        {
            ErrorCode = code;
            Detail = detail;
        }
    }
}
