using DotNet_EntityFrameworkCore.Core;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using DotNet_EntityFrameworkCore.WebAPICore;

namespace DotNet_EntityFrameworkCore.WebAPICore
{
    public class HttpResponseExceptionFilter : IActionFilter, IOrderedFilter
    {
        ILogger<HttpResponseExceptionFilter> _log;
        public HttpResponseExceptionFilter(ServiceInfo serviceInfo, ILogger<HttpResponseExceptionFilter> log)
        {
            this.serviceInfo = serviceInfo;
            _log = log;
        }
        ServiceInfo serviceInfo;
        public int Order { get; } = int.MaxValue - 10;

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception != null)
            {
                var actionDescriptor = ((ControllerBase)context.Controller)
                                           .ControllerContext
                                           .ActionDescriptor;

                string _controllerName = actionDescriptor?.ControllerTypeInfo.Name.ToString();
                string _methodName = actionDescriptor?.ActionName;
                string msg = string.Format("{0} | {1} | => {2}", _controllerName, _methodName, context.Exception.ToString());

                var genericMsg = "An error occurred";

                string moduleCode = this.serviceInfo?.ServiceCode ?? "XX";
                string EnvironmentCode = this.serviceInfo?.Environment ?? "Development";
                if (context.Exception is DataErrorException)
                {
                    var ex = context.Exception as DataErrorException;
                    if (ex.ErrorCode == DataErrorCode.Code09_InvalidRelatedData)
                        context.Result = new ObjectResult(new APIResultError(
                            new APIErrorDetail()
                            {
                                Detail = ex.Detail,
                                Message = ex.Message,
                                ErrorCode = $"{moduleCode}0{ex.ErrorCode}"
                            }, 400))
                        {
                            StatusCode = 400
                        };
                    else
                        context.Result = new ObjectResult(new APIResultError(
                                new APIErrorDetail()
                                {
                                    Detail = ex.Detail,
                                    Message = ex.Message,
                                    ErrorCode = $"{moduleCode}0{ex.ErrorCode}"
                                }, 210))
                        {
                            StatusCode = 210
                        };
                    context.ExceptionHandled = true;
                }
                ///TODO: Modify
                else if (context.Exception is UpdateRecordFailedException)
                {
                    var ex = context.Exception as UpdateRecordFailedException;
                    context.Result = new ObjectResult(new APIResultError(
                            new APIErrorDetail()
                            {
                                Message = EnvironmentCode == "Development" || EnvironmentCode == "SIT" ? ex.Message : genericMsg,
                                ErrorCode = $"{moduleCode}103",
                                Detail = EnvironmentCode == "Development" || EnvironmentCode == "SIT" ? context.Exception.ToString() : genericMsg
                            }, 500))
                    {
                        StatusCode = 500
                    };
                    _log.LogError(msg);
                    context.ExceptionHandled = true;
                }
                else if (context.Exception is BadHttpRequestException)
                {
                    var ex = context.Exception as BadHttpRequestException;
                    context.Result = new ObjectResult(new APIResultError(
                            new APIErrorDetail()
                            {
                                Message = ex.Message,
                                ErrorCode = $"{moduleCode}010",
                                Detail = context.Exception.ToString()
                            }, 400))
                    {
                        StatusCode = 400
                    };
                    _log.LogError(msg);
                    context.ExceptionHandled = true;
                }
                else if (context.Exception is DeleteRecordFailedException)
                {
                    var ex = context.Exception as DeleteRecordFailedException;
                    context.Result = new ObjectResult(new APIResultError(
                            new APIErrorDetail()
                            {
                                Message = EnvironmentCode == "Development" || EnvironmentCode == "SIT" ? ex.Message : genericMsg,
                                ErrorCode = $"{moduleCode}104",
                                Detail = EnvironmentCode == "Development" || EnvironmentCode == "SIT" ? context.Exception.ToString() : genericMsg
                            }, 500))
                    {
                        StatusCode = 500
                    };
                    _log.LogError(msg);
                    context.ExceptionHandled = true;
                }
                else if (context.Exception is ResourceNotFoundException)
                {
                    context.Result = new ObjectResult(new APIResultError(
                            new APIErrorDetail()
                            {
                                Message = "Not Found",
                                ErrorCode = $"{moduleCode}008",
                                Detail = context.Exception.ToString()
                            }, 404))
                    {
                        StatusCode = 404
                    };
                    _log.LogError(msg);
                    context.ExceptionHandled = true;
                }
                else if (context.Exception is DataNotFoundException)
                {
                    context.Result = new ObjectResult(new APIResultError(
                            new APIErrorDetail()
                            {
                                Message = context.Exception.Message ?? "Not Found Data",
                                ErrorCode = $"{moduleCode}003",
                                Detail = context.Exception.ToString()
                            }, 404))
                    {
                        StatusCode = 404
                    };
                    //_log.LogError(msg);
                    context.ExceptionHandled = true;
                }
                else if (context.Exception.InnerException is MySqlConnector.MySqlException)
                {
                    context.Result = new ObjectResult(new APIResultError(
                            new APIErrorDetail()
                            {
                                Message = "Database connection failed.",
                                ErrorCode = $"{moduleCode}101",
                                Detail = EnvironmentCode == "Development" || EnvironmentCode == "SIT" ? context.Exception.ToString() : genericMsg
                            }, 500))
                    {
                        StatusCode = 500
                    };
                    _log.LogError(msg);
                    context.ExceptionHandled = true;
                }
                else if (context.Exception is UnauthorizedException)
                {
                    context.Result = new ObjectResult(new APIResultError(
                            new APIErrorDetail()
                            {
                                Message = "Unauthorized",
                                ErrorCode = $"{moduleCode}303",
                                Detail = context.Exception.ToString()
                            }, 401))
                    {
                        StatusCode = 401
                    };
                    _log.LogError(msg);
                    context.ExceptionHandled = true;
                }
                else if (context.Exception is PermissionDeniedException)
                {
                    context.Result = new ObjectResult(new APIResultError(
                            new APIErrorDetail()
                            {
                                Message = "Forbidden",
                                ErrorCode = $"{moduleCode}304",
                                Detail = context.Exception.ToString()
                            }, 403))
                    {
                        StatusCode = 403
                    };
                    _log.LogError(msg);
                    context.ExceptionHandled = true;
                }
                else if (context.Exception is ApiErrorException)
                {
                    var ex = context.Exception as ApiErrorException;
                    context.Result = new ObjectResult(new APIResultError(
                            new APIErrorDetail()
                            {
                                Message = ex.Message,
                                ErrorCode = ex.ErrorCode,
                                Detail = ex.Detail
                            }, ex.HttpStatus))
                    {
                        StatusCode = ex.HttpStatus
                    };
                    _log.LogError(msg);
                    context.ExceptionHandled = true;
                }
                else
                {
                    context.Result = new ObjectResult(new APIResultError(
                            new APIErrorDetail()
                            {
                                Message = genericMsg,
                                ErrorCode = $"{moduleCode}301",
                                Detail = EnvironmentCode == "Development" || EnvironmentCode == "SIT" ? context.Exception.ToString() : genericMsg
                            }, 500))
                    {
                        StatusCode = 500
                    };

                    context.ExceptionHandled = true;
                    _log.LogError(msg);
                }


            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {

        }
    }
}
