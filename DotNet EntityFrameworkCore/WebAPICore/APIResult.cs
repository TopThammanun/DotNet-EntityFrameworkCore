using DotNet_EntityFrameworkCore.Core;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet_EntityFrameworkCore.WebAPICore
{
    public class APIResult
    {
        public int Status { get; set; }
        public bool Success { get; set; }
        public string Version { get; set; } = AppSettings.Get<string>("Version");
        public object Data { get; set; }
    }

    public class APIResult<TData> where TData : class, new()
    {
        public int Status { get; set; }
        public bool Success { get; set; }
        public TData Data { get; set; }
    }

    public class APIResultSuccess : APIResult
    {
        public APIResultSuccess()
        {
            Status = 200;
            Data = null;
            Success = true;
        }

        public APIResultSuccess(object data, int status)
        {
            Status = status;
            Data = data;
            Success = true;
        }

        public APIResultSuccess(object data)
        {
            Status = 200;
            Data = data;
            Success = true;
        }
    }

    public class APIResultSuccess<TData> : APIResult<TData> where TData : class, new()
    {
        public APIResultSuccess()
        {
            Status = 200;
            Data = null;
            Success = true;
        }

        public APIResultSuccess(TData data, int status)
        {
            Status = status;
            Data = data;
            Success = true;
        }

        public APIResultSuccess(TData data)
        {
            Status = 200;
            Data = data;
            Success = true;
        }
    }

    public class APIResultError : APIResult
    {
        public APIResultError(APIErrorDetail detail)
        {
            Data = detail;
            Status = 500;
            Success = false;
        }

        public APIResultError(APIErrorDetail detail, int status)
        {
            Data = detail;
            Status = status;
            Success = false;
        }
    }

    public class APIErrorDetail
    {
        public string ErrorCode { get; set; }
        public string Message { get; set; }
        public string Detail { get; set; }
    }

    public class ValidationErrorData
    {
        public string FieldName { get; set; }
        public string[] Messages { get; set; }
    }
    public class ValidationErrorDetail
    {
        public string ErrorCode { get; set; }
        public string Message { get; set; }
        public string Detail { get; set; }
        public List<ValidationErrorData> Errors { get; set; }
    }
}
