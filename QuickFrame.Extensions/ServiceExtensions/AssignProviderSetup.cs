using QuickFrame.Common;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 赋值器配置
    /// </summary>
    public static class AssignProviderSetup
    {
        /// <summary>
        /// 注入赋值器配置
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddAssignProviderSetup(this IServiceCollection services)
        {
            return services.AddSingleton(provider => provider.GetRequiredService<IAssignProvider>().CreateAssign());
        }
    }
}
