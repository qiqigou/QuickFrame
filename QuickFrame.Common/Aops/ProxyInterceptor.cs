using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Castle.DynamicProxy;

namespace QuickFrame.Common
{
    /// <summary>
    /// 实现统一的拦截器
    /// </summary>
    [SingletonInjection]
    public class ProxyInterceptor : IProxyInterceptor
    {
        private readonly IServiceProvider _serviceProvide;

        public ProxyInterceptor(IServiceProvider serviceProvider)
        {
            _serviceProvide = serviceProvider;
        }

        public void Intercept(IInvocation invocation)
        {
            var method = invocation.MethodInvocationTarget ?? invocation.Method;
            var attr = method.GetCustomAttribute<AutoProxyAttribute>();
            if (attr != null)
            {
                var scope = _serviceProvide.GetAutofacRoot();
                var handles = attr.Options.Distinct().Select(x => scope.ResolveNamed<IProxyHandle>(x)).ToList();
                handles.ForEach(x => x.InterceptAction());
                invocation.Proceed();
            }
            else
            {
                invocation.Proceed();
            }
        }

    }
}
