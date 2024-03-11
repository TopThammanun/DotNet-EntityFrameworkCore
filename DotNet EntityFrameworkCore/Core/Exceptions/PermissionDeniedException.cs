using System;
using System.Collections.Generic;
using System.Text;

namespace DotNet_EntityFrameworkCore.Core
{
    public class PermissionDeniedException : Exception
    {
        public PermissionDeniedException()
            : base("Permission Denied")
        {
            
        }
        public PermissionDeniedException(string message)
            : base(message)
        {

        }
    }
}
