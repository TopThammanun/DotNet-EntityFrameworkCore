using System;
using System.Collections.Generic;
using System.Text;

namespace DotNet_EntityFrameworkCore.Core
{
    public class DataValidationException : Exception
    {
        /// <summary>
        /// Thow invalid data message with string format
        /// </summary>
        /// <param name="message"></param>
        /// <param name="parameters"></param>
        public DataValidationException(string message, params string[] parameters)
            : base(string.Format(message, parameters))
        {

        }
    }
}
