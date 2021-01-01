using Microsoft.AspNetCore.Mvc.Filters;
using QuickFrame.Common;
using System.Threading.Tasks;

namespace QuickFrame.Web
{
    /// <summary>
    /// API方法拦截器
    /// </summary>
    public class ApiActionFilter : IActionFilter, IAsyncActionFilter
    {
        /// <summary>
        /// 方法执行前拦截
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuting(ActionExecutingContext context) => DefaultOption.EmptyFunc();
        /// <summary>
        /// 方法执行后拦截
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuted(ActionExecutedContext context) => DefaultOption.EmptyFunc();
        /// <summary>
        /// 实现异步接口
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            OnActionExecuting(context);
            OnActionExecuted(await next.Invoke());
        }
    }
}
