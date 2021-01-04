using QuickFrame.Common;
using QuickFrame.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuickFrame.Repositorys
{
    /// <summary>
    /// 提供通用的视图查询
    /// </summary>
    public interface IQueryProvider<TEntity, TKey>
        where TEntity : ViewEntity, new()
        where TKey : notnull
    {
        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="qkeyValue">查询键值</param>
        /// <returns></returns>
        Task<TEntity?> SingleAsync(TKey qkeyValue);
        /// <summary>
        /// 获取集合
        /// </summary>
        /// <param name="qkeyValue">查询键值</param>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> QueryAsync(TKey qkeyValue);
        /// <summary>
        /// 获取集合
        /// </summary>
        /// <param name="arrayKeyValue">查询键值集合</param>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> QueryAsync(TKey[] arrayKeyValue);
        /// <summary>
        /// 条件查询
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PageOutput<TEntity>> QueryAsync(ObjFilterInput input);
        /// <summary>
        /// 条件查询
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PageOutput<TEntity>> QueryAsync(SQLFilterInput input);
    }
}
