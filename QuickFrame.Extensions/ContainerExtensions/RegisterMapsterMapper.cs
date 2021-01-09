using Autofac;
using Mapster;
using MapsterMapper;
using System.Reflection;

namespace QuickFrame.Extensions
{
    /// <summary>
    /// 映射器配置
    /// </summary>
    public static class RegisterMapsterMapper
    {
        /// <summary>
        /// 注入映射器配置
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="assemblys"></param>
        /// <returns></returns>
        public static ContainerBuilder AddRegisterMapsterMapper(this ContainerBuilder builder, params Assembly[] assemblys)
        {
            //映射器注入
            var config = new TypeAdapterConfig();
            config.Default.NameMatchingStrategy(NameMatchingStrategy.Flexible)
            .ShallowCopyForSameType(false)
            .IgnoreNullValues(true)
            .Config
            .Scan(assemblys);
            builder.RegisterInstance(config).SingleInstance();
            builder.RegisterType<ServiceMapper>().As<IMapper>().InstancePerLifetimeScope();
            return builder;
        }
    }
}
