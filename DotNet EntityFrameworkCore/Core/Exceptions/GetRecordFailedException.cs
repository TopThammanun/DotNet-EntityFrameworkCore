using System;

namespace DotNet_EntityFrameworkCore.Core
{
    public class GetRecordFailedException : Exception
    {
        public GetRecordFailedException(string message, Exception innerException) : base(message, innerException)
        {

        }

        public GetRecordFailedException(Exception innerException) : base("Get Data Failed!", innerException)
        {

        }
    }
}