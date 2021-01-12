using Microsoft.AspNetCore.Hosting;
using QuickFrame.Common;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Hosting;

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// swagger文档中间件
    /// </summary>
    public static class SwaggerMildd
    {
        /// <summary>
        /// swagger文档中间件
        /// </summary>
        /// <param name="app"></param>
        /// <param name="environment"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseSwaggerMildd(this IApplicationBuilder app, IWebHostEnvironment environment)
        {
            var appconfig = app.ApplicationServices.GetRequiredService<IOptions<AppConfig>>().Value;
            if (!environment.IsDevelopment() && !appconfig.Swagger) return app;
            app.UseSwagger();
            app.UseSwaggerUI(doc =>
            {
                doc.DocumentTitle = $"{AssemblyOption.RootName} {Assembly.GetEntryAssembly()?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion}";
                foreach (var item in ConstantOptions.ModulesConstant.Modules)
                {
                    doc.SwaggerEndpoint($"swagger/{item}/swagger.json", $"api-{item}");
                }
                doc.RoutePrefix = string.Empty;//设置swagger页面访问别名
                doc.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);//折叠Api
                doc.DefaultModelsExpandDepth(1);//显示Models
            });
            return app;
        }
    }
}
