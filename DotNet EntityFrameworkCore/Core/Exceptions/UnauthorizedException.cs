using System;
using System.Collections.Generic;
using System.Text;

namespace DotNet_EntityFrameworkCore.Core
{
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException()
            : base("Unauthorized")
        {

        }
        public UnauthorizedException(string message)
            : base(message)
        {

        }
    }
}
