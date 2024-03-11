using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet_EntityFrameworkCore.Core
{
    public class ApiErrorException : Exception
    {
        public int HttpStatus { get; }
        public string ErrorCode { get; }
        public string Detail { get; }
        public ApiErrorException(int httpStatus, string code, string message)
            : base(message)
        {
            HttpStatus = httpStatus;
            ErrorCode = code;
        }

        public ApiErrorException(int httpStatus, string code, string message, string detail)
            : base(message)
        {
            HttpStatus = httpStatus;
            ErrorCode = code;
            Detail = detail;
        }
    }
}
