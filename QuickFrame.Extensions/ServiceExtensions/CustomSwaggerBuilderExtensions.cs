using QuickFrame.Common;
using System.Reflection;

namespace Microsoft.AspNetCore.Builder
{
    public static class CustomSwaggerBuilderExtensions
    {
        public static IApplicationBuilder UseCustomSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(doc =>
            {
                doc.DocumentTitle = $"{AssemblyOption.RootName} {Assembly.GetEntryAssembly()?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion}";
                foreach (var item in ConstantOptions.ModulesConstant.Modules)
                {
                    doc.SwaggerEndpoint($"/swagger/{item}/swagger.json", $"api-{item}");
                }
                doc.RoutePrefix = string.Empty;//设置swagger页面访问别名
                doc.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);//折叠Api
                doc.DefaultModelsExpandDepth(1);//显示Models
            });
            return app;
        }
    }
}
