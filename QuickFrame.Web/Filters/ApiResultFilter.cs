using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;
using QuickFrame.Common;

namespace QuickFrame.Web
{
    /// <summary>
    /// API结果过滤器
    /// </summary>
    public class ApiResultFilter : IResultFilter, IAsyncResultFilter
    {
        /// <summary>
        /// 生成结果前拦截
        /// </summary>
        /// <param name="context"></param>
        public void OnResultExecuting(ResultExecutingContext context) => DefaultOption.EmptyFunc();
        /// <summary>
        /// 生成结果后拦截
        /// </summary>
        /// <param name="context"></param>
        public void OnResultExecuted(ResultExecutedContext context) => DefaultOption.EmptyFunc();
        /// <summary>
        /// 实现异步接口
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            OnResultExecuting(context);
            OnResultExecuted(await next.Invoke());
        }
    }
}