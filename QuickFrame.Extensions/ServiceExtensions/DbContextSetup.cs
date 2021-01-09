using QuickFrame.Models;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 数据库上下文配置
    /// </summary>
    public static class DbContextSetup
    {
        /// <summary>
        /// 注入数据库上下文配置
        /// </summary>
        /// <param name="services"></param>
        public static IServiceCollection AddDbContextSetup(this IServiceCollection services)
        {
            services.AddDbContext<BackDbContext>(ServiceLifetime.Scoped, ServiceLifetime.Scoped);//注入后台库
            services.AddDbContext<WorkDbContext>(ServiceLifetime.Scoped, ServiceLifetime.Scoped);//注入业务库
            return services;
        }
    }
}
