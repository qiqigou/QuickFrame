using Microsoft.Extensions.Configuration;
using QuickFrame.Common;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 跨域配置
    /// </summary>
    public static class CorsSetup
    {
        /// <summary>
        /// 注入跨域配置
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddCorsSetup(this IServiceCollection services, IConfiguration configuration)
        {
            var appConfig = configuration.Get<AppConfig>();
            return services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy
                    .WithOrigins(appConfig.CorUrls)
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
                });
            });
        }
    }
}
