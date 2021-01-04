using QuickFrame.Common;
using QuickFrame.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuickFrame.Services
{
    /// <summary>
    /// 单视图查询
    /// </summary>
    /// <typeparam name="TView">视图</typeparam>
    /// <typeparam name="TQKey">查询键</typeparam>
    public interface IQuery<TView, TQKey>
        where TView : ViewEntity, new()
        where TQKey : notnull
    {
        /// <summary>
        /// 获取单条数据
        /// </summary>
        /// <param name="qkeyValue"></param>
        /// <returns></returns>
        Task<TView?> SingleAsync(TQKey qkeyValue);
        /// <summary>
        /// 获取集合
        /// </summary>
        /// <param name="qkeyValue"></param>
        /// <returns></returns>
        Task<IEnumerable<TView>> QueryAsync(TQKey qkeyValue);
        /// <summary>
        /// 条件查询
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PageOutput<TView>> ObjQueryAsync(ObjFilterInput input);
        /// <summary>
        /// 条件查询
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PageOutput<TView>> SQLQueryAsync(SQLFilterInput input);
    }
}
