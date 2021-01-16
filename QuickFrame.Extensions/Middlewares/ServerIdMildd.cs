using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using QuickFrame.Common;

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// ServerID中间件
    /// </summary>
    public static class ServerIdMildd
    {
        /// <summary>
        /// ServerID中间件
        /// </summary>
        /// <param name="app"></param>
        /// <param name="_configuration"></param>
        public static IApplicationBuilder UseServerIdMildd(this IApplicationBuilder app, IConfiguration _configuration)
        {
            var appconfig = _configuration.Get<AppConfig>();
            return app.Map("/serverid", builder => builder.Run(async context =>
            {
                context.Response.ContentType = "text/html; charset=utf-8";
                await context.Response.WriteAsync(appconfig.ServerId);
            }));
        }
    }
}
