using System;
using System.Collections.Generic;
using System.Text;

namespace DotNet_EntityFrameworkCore.Core
{
    public class ResourceNotFoundException : Exception
    {
        public ResourceNotFoundException()
            : base("Not Found")
        {

        }
        public ResourceNotFoundException(string message)
            : base(message)
        {

        }
    }
}
