using Autofac;
using Autofac.Extras.DynamicProxy;
using QuickFrame.Common;
using QuickFrame.IServices;
using QuickFrame.Repositorys;
using System.Linq;
using System.Reflection;

namespace QuickFrame.Extensions
{
    /// <summary>
    /// 服务和仓储配置
    /// </summary>
    public static class RegisterServicesAndRepositorySetup
    {
        /// <summary>
        /// 注入服务和仓储配置
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static ContainerBuilder AddRegisterServicesAndRepository(this ContainerBuilder builder)
        {
            var assemblyServices = Assembly.Load(AssemblyOption.ServicesName);
            var assemblyRepository = Assembly.Load(AssemblyOption.RepositorysName);

            //服务注入
            builder.RegisterAssemblyTypes(assemblyServices)
            .Where(x => x.IsAssignableTo<IService>())
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope()
            .InterceptedBy(typeof(IProxyInterceptor))
            .EnableInterfaceInterceptors();//启用动态代理

            //服务设置动态代理
            assemblyServices.GetTypes()
            .Where(x => x.IsAssignableTo(typeof(IProxyHandle)))
            .ForEach(item => builder.RegisterType(item).Named<IProxyHandle>(item.Name)
            .InstancePerLifetimeScope());

            //仓储注入
            builder.RegisterAssemblyTypes(assemblyRepository)
            .Where(x => x.IsAssignableTo<IRepository>())
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();
            return builder;
        }
    }
}
