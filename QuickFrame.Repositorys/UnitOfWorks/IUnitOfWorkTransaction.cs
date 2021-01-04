using System;
using System.Data.Common;
using System.Threading.Tasks;

namespace QuickFrame.Repositorys
{
    /// <summary>
    /// 工作单元事务
    /// </summary>
    public interface IUnitOfWorkTransaction : IDisposable, IAsyncDisposable
    {
        /// <summary>
        /// 事务ID
        /// </summary>
        Guid TransactionId { get; }
        /// <summary>
        /// 当前事务
        /// </summary>
        DbTransaction Transaction { get; }
        /// <summary>
        /// 提交事务
        /// </summary>
        /// <returns></returns>
        void Commit();
        /// <summary>
        /// 提交事务(异步)
        /// </summary>
        /// <returns></returns>
        Task CommitAsync();
        /// <summary>
        /// 回滚事务
        /// </summary>
        void Rollback();
        /// <summary>
        /// 回滚事务(异步)
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task RollbackAsync();
    }
}
