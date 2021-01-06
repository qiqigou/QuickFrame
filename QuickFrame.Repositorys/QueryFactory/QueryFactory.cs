using Microsoft.Extensions.DependencyInjection;
using QuickFrame.Common;
using QuickFrame.Models;
using System;
using System.Linq.Expressions;

namespace QuickFrame.Repositorys
{
    /// <summary>
    /// 查询提供程序工厂类
    /// </summary>
    [SingletonInjection]
    public class QueryFactory : IQueryFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public QueryFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        /// <summary>
        /// 构建视图查询提供者
        /// </summary>
        /// <remarks>
        /// 1.查询键与主键的概念不同,本项目中视图没有主键的概念,取而代之的是查询键
        /// 2.查询键可以任意指定.(在服务抽象类中已经托管了,所以需要与服务类的主键保持一致)
        /// 3.查询键可以通过元组表达式设置多个键.例如:qkeys => new(qkeys.Name, qkeys.Age)
        /// 4.查询键可以通过匿名表达式设置多个键.例如:orderkeys => new { orderkeys.Name, orderkeys.Age }
        /// </remarks>
        /// <typeparam name="IOption">库标志</typeparam>
        /// <typeparam name="TView">视图类型</typeparam>
        /// <typeparam name="TQKey">查询键类型</typeparam>
        /// <param name="qkeys">查询键</param>
        /// <param name="orderkeys">排序键</param>
        /// <returns></returns>
        public IQueryProvider<TView, TQKey> Create<IOption, TView, TQKey>(Expression<Func<TView, TQKey>> qkeys, Expression<Func<TView, object>>? orderkeys = default)
            where IOption : IContextOption
            where TView : ViewEntity, new()
            where TQKey : notnull
        {
            var queryRepository = _serviceProvider.GetRequiredService<IQueryRepository<IOption>>();
            var qkeyNames = ExpressionHelper.GetMemberNames(qkeys);
            var orderkeyNames = orderkeys == default ? qkeyNames : ExpressionHelper.GetMemberNames(orderkeys);
            return new QueryProvider<TView, TQKey>(queryRepository, qkeyNames, orderkeyNames);
        }
        /// <summary>
        /// 构建视图查询提供者
        /// </summary>
        /// <typeparam name="IOption">库标志</typeparam>
        /// <typeparam name="TView">视图类型</typeparam>
        /// <typeparam name="TQKey">查询键类型</typeparam>
        /// <param name="qkeys">查询键</param>
        /// <param name="orderkeys">排序键</param>
        /// <returns></returns>
        public IQueryProvider<TView, TQKey> Create<IOption, TView, TQKey>(Expression<Func<TView, TQKey>> qkeys, SortInput[] orderkeys)
            where IOption : IContextOption
            where TView : ViewEntity, new()
            where TQKey : notnull
        {
            var queryRepository = _serviceProvider.GetRequiredService<IQueryRepository<IOption>>();
            var qkeyNames = ExpressionHelper.GetMemberNames(qkeys);
            return new QueryProvider<TView, TQKey>(queryRepository, qkeyNames, orderkeys);
        }
    }
}
