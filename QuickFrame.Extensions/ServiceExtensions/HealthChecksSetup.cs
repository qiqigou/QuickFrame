using QuickFrame.Models;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 健康检查配置
    /// </summary>
    public static class HealthChecksSetup
    {
        /// <summary>
        /// 注入健康检查配置
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddHealthChecksSetup(this IServiceCollection services)
        {
            services.AddHealthChecks()
               .AddDbContextCheck<BackDbContext>()
               .AddDbContextCheck<WorkDbContext>();
            return services;
        }
    }
}
