using Microsoft.AspNetCore.Authorization;
using QuickFrame.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 授权策略配置
    /// </summary>
    public static class AuthorizationPolicySetup
    {
        /// <summary>
        /// 注入授权策略配置
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddAuthorizationPolicySetup(this IServiceCollection services)
        {
            services.AddSingleton<IAuthorizationHandler, RootPermissionAuthorizationHandler>();//预设角色授权策略
            services.AddScoped<IAuthorizationHandler, ApiPermissionAuthorizationHandler>();//API权限授权策略
            return services;
        }
    }
}
