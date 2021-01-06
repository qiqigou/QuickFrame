using QuickFrame.Common;
using QuickFrame.Models;
using System;
using System.Linq.Expressions;

namespace QuickFrame.Repositorys
{
    /// <summary>
    /// 查询提供程序工厂接口
    /// </summary>
    public interface IQueryFactory
    {
        /// <summary>
        /// 构建视图查询提供者
        /// </summary>
        /// <typeparam name="IOption">库标志</typeparam>
        /// <typeparam name="TView">视图类型</typeparam>
        /// <typeparam name="TQKey">查询键类型</typeparam>
        /// <param name="qkeys">查询键</param>
        /// <param name="orderkeys">排序键</param>
        /// <returns></returns>
        IQueryProvider<TView, TQKey> Create<IOption, TView, TQKey>(Expression<Func<TView, TQKey>> qkeys, Expression<Func<TView, object>>? orderkeys = default)
            where IOption : IContextOption
            where TView : ViewEntity, new()
            where TQKey : notnull;
        /// <summary>
        /// 构建视图查询提供者
        /// </summary>
        /// <typeparam name="IOption">库标志</typeparam>
        /// <typeparam name="TView">视图类型</typeparam>
        /// <typeparam name="TQKey">查询键类型</typeparam>
        /// <param name="qkeys">查询键</param>
        /// <param name="orderkeys">排序键</param>
        /// <returns></returns>
        IQueryProvider<TView, TQKey> Create<IOption, TView, TQKey>(Expression<Func<TView, TQKey>> qkeys, SortInput[] orderkeys)
            where IOption : IContextOption
            where TView : ViewEntity, new()
            where TQKey : notnull;
    }
}
