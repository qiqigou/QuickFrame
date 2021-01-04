using QuickFrame.Common;
using QuickFrame.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuickFrame.Services
{
    /// <summary>
    /// 主子视图查询
    /// </summary>
    /// <typeparam name="TMainView">主视图</typeparam>
    /// <typeparam name="TChildView">子视图</typeparam>
    /// <typeparam name="TQKey">查询键</typeparam>
    public interface IQueryMainChild<TMainView, TChildView, TQKey>
        where TMainView : WithStampView, new()
        where TChildView : ViewEntity, new()
        where TQKey : notnull
    {
        /// <summary>
        /// 获取主视图单条数据
        /// </summary>
        /// <param name="qkeyValue"></param>
        /// <returns></returns>
        Task<TMainView?> SingleMainAsync(TQKey qkeyValue);
        /// <summary>
        /// 获取主子视图数据
        /// </summary>
        /// <param name="qkeyValue"></param>
        /// <returns></returns>
        Task<MainChildOutput<TMainView, TChildView>?> SingleMainChildAsync(TQKey qkeyValue);
        /// <summary>
        /// 获取子视图集合
        /// </summary>
        /// <param name="qkeyValue"></param>
        /// <returns></returns>
        Task<IEnumerable<TChildView>> QueryChildAsync(TQKey qkeyValue);
        /// <summary>
        /// 主视图条件查询
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PageOutput<TMainView>> ObjQueryMainAsync(ObjFilterInput input);
        /// <summary>
        /// 主视图条件查询
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PageOutput<TMainView>> SQLQueryMainAsync(SQLFilterInput input);
    }
}
