using QuickFrame.Common;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class CustomCorsServiceCollectionExtensions
    {
        /// <summary>
        /// 跨域配置
        /// </summary>
        /// <param name="services"></param>
        /// <param name="appConfig"></param>
        /// <returns></returns>
        public static IServiceCollection AddCustomCors(this IServiceCollection services, AppConfig appConfig)
        {
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
