using System.Linq;
using System.Reflection;
using Autofac;
using QuickFrame.Common;

namespace QuickFrame.Extensions
{
    /// <summary>
    /// 特性注入配置
    /// </summary>
    public static class RegisterAttributes
    {
        /// <summary>
        /// 注入所有被特性标记的类
        /// </summary>
        public static ContainerBuilder AddRegisterAttributeSetup(this ContainerBuilder builder)
        {
            var assemblyServices = Assembly.Load(AssemblyOption.ServicesName);
            var assemblyCommon = Assembly.Load(AssemblyOption.CommonName);
            var assemblyRepository = Assembly.Load(AssemblyOption.RepositorysName);

            //单例注入
            builder.RegisterAssemblyTypes(assemblyServices, assemblyCommon, assemblyRepository)
            .Where(x => x.GetCustomAttribute<SingletonInjectionAttribute>() != default)
            .AsImplementedInterfaces()
            .SingleInstance();

            //范围注入
            builder.RegisterAssemblyTypes(assemblyServices, assemblyCommon, assemblyRepository)
            .Where(x => x.GetCustomAttribute<ScopeInjectionAttribute>() != default)
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();

            //瞬态注入
            builder.RegisterAssemblyTypes(assemblyServices, assemblyCommon, assemblyRepository)
            .Where(x => x.GetCustomAttribute<TransientInjectionAttribute>() != default)
            .AsImplementedInterfaces()
            .InstancePerDependency();
            return builder;
        }
    }
}
