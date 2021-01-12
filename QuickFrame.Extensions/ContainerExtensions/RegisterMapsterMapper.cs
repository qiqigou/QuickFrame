using Autofac;
using Mapster;
using MapsterMapper;
using QuickFrame.Common;
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
        /// <returns></returns>
        public static ContainerBuilder AddRegisterMapsterMapper(this ContainerBuilder builder)
        {
            var assemblyServices = Assembly.Load(AssemblyOption.ServicesName);
            var assemblyCommon = Assembly.Load(AssemblyOption.CommonName);
            var assemblyRepository = Assembly.Load(AssemblyOption.RepositorysName);

            //映射器注入
            var config = new TypeAdapterConfig();
            config.Default.NameMatchingStrategy(NameMatchingStrategy.Flexible)
            .ShallowCopyForSameType(false)
            .IgnoreNullValues(true)
            .Config
            .Scan(assemblyServices, assemblyCommon, assemblyRepository);
            builder.RegisterInstance(config).SingleInstance();
            builder.RegisterType<ServiceMapper>().As<IMapper>().InstancePerLifetimeScope();
            return builder;
        }
    }
}
