using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using QuickFrame.Common;
using System.IO;

namespace QuickFrame.Extensions
{
    /// <summary>
    /// ServerID中间件
    /// </summary>
    public static class ServerIdMildd
    {
        public static void UseServerIdMildd(this IApplicationBuilder app, IConfiguration _configuration)
        {
            var appconfig = _configuration.Get<AppConfig>();
            app.Map("/serverid", builder => builder.Run(async context =>
            {
                context.Response.ContentType = "text/html; charset=utf-8";
                await context.Response.WriteAsync(appconfig.ServerId);
            }));
        }
    }
}
