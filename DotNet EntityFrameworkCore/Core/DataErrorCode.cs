using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet_EntityFrameworkCore.Core
{
    public class DataErrorCode
    {
        public const string Code01_NotFoundDataToUpdate = "01";
        public const string Code02_NotFoundDataToDelete = "02";
        public const string Code03_NotFoundData = "03";
        public const string Code04_DataInvalid = "04";
        public const string Code05_DuplicatePrimaryKey = "05";
        public const string Code06_InvalidUsername = "06";
        public const string Code07_LoginFailed = "07";
        public const string Code09_InvalidRelatedData = "09";
        public const string Code10_DataWasReferencedCannotDelete = "10";
        public const string Code401_Unsupport = "401";
    }
}
