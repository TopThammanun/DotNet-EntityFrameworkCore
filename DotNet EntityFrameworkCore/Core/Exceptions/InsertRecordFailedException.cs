using System;

namespace DotNet_EntityFrameworkCore.Core
{
    public class InsertRecordFailedException : Exception
    {
        public InsertRecordFailedException(string message, Exception innerException) : base(message, innerException)
        {

        }

        public InsertRecordFailedException(Exception innerException) : base("Insert Data Failed!", innerException)
        {

        }
    }
}