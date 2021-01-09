using Autofac;
using QuickFrame.Common;
using System.Linq;
using System.Reflection;

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
        public static ContainerBuilder AddRegisterAttributeSetup(this ContainerBuilder builder, params Assembly[] assemblys)
        {
            //单例注入
            builder.RegisterAssemblyTypes(assemblys)
            .Where(x => x.GetCustomAttribute<SingletonInjectionAttribute>() != default)
            .AsImplementedInterfaces()
            .SingleInstance();

            //范围注入
            builder.RegisterAssemblyTypes(assemblys)
            .Where(x => x.GetCustomAttribute<ScopeInjectionAttribute>() != default)
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();

            //瞬态注入
            builder.RegisterAssemblyTypes(assemblys)
            .Where(x => x.GetCustomAttribute<TransientInjectionAttribute>() != default)
            .AsImplementedInterfaces()
            .InstancePerDependency();
            return builder;
        }
    }
}
