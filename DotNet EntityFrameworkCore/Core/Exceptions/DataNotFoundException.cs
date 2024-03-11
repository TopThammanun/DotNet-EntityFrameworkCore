using System;

namespace DotNet_EntityFrameworkCore.Core
{
    public class DataNotFoundException : Exception
    {
        public DataNotFoundException() : base("Data Not Found")
        {

        }
        public DataNotFoundException(string message) : base(message)
        {

        }
    }
}
