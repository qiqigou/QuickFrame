using QuickFrame.Model;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace QuickFrame.Repository
{
    /// <summary>
    /// 视图查询仓储
    /// </summary>
    /// <typeparam name="TOption">DbContext类别</typeparam>
    public interface IQueryRepository<TOption> : IQueryRepository where TOption : IContextOption { }
    /// <summary>
    /// 视图查询仓储
    /// </summary>
    public interface IQueryRepository : IRepository
    {
        /// <summary>
        /// 查询(未跟踪)
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        IQueryable<TEntity> Select<TEntity>() where TEntity : ViewEntity, new();
        /// <summary>
        /// 是否有一项满足条件
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<bool> AnyAsync<TEntity>(Expression<Func<TEntity, bool>>? predicate = default) where TEntity : ViewEntity, new();
        /// <summary>
        /// 是否有一项满足条件
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        bool Any<TEntity>(Expression<Func<TEntity, bool>>? predicate = default) where TEntity : ViewEntity, new();
        /// <summary>
        /// 计数(异步)
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<int> CountAsync<TEntity>(Expression<Func<TEntity, bool>>? predicate = default) where TEntity : ViewEntity, new();
        /// <summary>
        /// 计数
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        int Count<TEntity>(Expression<Func<TEntity, bool>>? predicate = default) where TEntity : ViewEntity, new();
    }
}
