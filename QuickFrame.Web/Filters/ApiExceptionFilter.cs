using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using QuickFrame.Common;

namespace QuickFrame.Web
{
    /// <summary>
    /// API异常过滤器
    /// </summary>
    public class ApiExceptionFilter : IExceptionFilter, IAsyncExceptionFilter
    {
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<ApiExceptionFilter> _logger;

        public ApiExceptionFilter(IWebHostEnvironment env, ILogger<ApiExceptionFilter> logger)
        {
            _environment = env;
            _logger = logger;
        }
        /// <summary>
        /// 发生异常时触发
        /// </summary>
        /// <param name="context"></param>
        public void OnException(ExceptionContext context)
        {
            switch (context.Exception)
            {
                case HandelException ex:
                    var result = new MsgOutput(ex.MsgCode)
                    {
                        MsgDetail = new[] { ex.Message }
                    };
                    context.Result = new BadRequestObjectResult(result);
                    _logger.LogWarning(LogEventsOption.Bad, ex.Message);
                    break;
                case HandelArrayException ex:
                    result = new MsgOutput(ex.Exceptions[0].MsgCode)
                    {
                        MsgDetail = ex.ArrayMessage
                    };
                    context.Result = new BadRequestObjectResult(result);
                    _logger.LogWarning(LogEventsOption.Bad, ex.Message);
                    break;
                case TaskCanceledException ex:
                    result = new MsgOutput(MessageCodeOption.Cancel)
                    {
                        MsgDetail = new[] { ex.Message }
                    };
                    context.Result = new BadRequestObjectResult(result);
                    _logger.LogWarning(LogEventsOption.Cancel, ex, MessageCodeOption.Cancel.Title);
                    break;
                default:
                    result = new MsgOutput(MessageCodeOption.Error);
                    if (_environment.IsDevelopment())
                    {
                        result.MsgDetail = new[] { context.Exception.ToString() };
                    }
                    else
                    {
                        result.MsgDetail = new[] { context.Exception.Message };
                    }
                    context.Result = new ObjectResult(result)
                    {
                        StatusCode = StatusCodes.Status500InternalServerError
                    };
                    _logger.LogError(LogEventsOption.Error, context.Exception, MessageCodeOption.Error.Title);
                    break;
            }
        }
        /// <summary>
        /// 发生异常时触发(异步)
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task OnExceptionAsync(ExceptionContext context)
        {
            OnException(context);
            return Task.CompletedTask;
        }
    }
}
