using Microsoft.EntityFrameworkCore.Storage;
using QuickFrame.Common;
using System;
using System.Data.Common;
using System.Threading.Tasks;

namespace QuickFrame.Repositorys
{
    /// <summary>
    /// 工作单元事务
    /// </summary>
    internal class UnitOfWorkTransaction : IUnitOfWorkTransaction
    {
        private IDbContextTransaction? _contextTran;
        /// <summary>
        /// 上下文事务
        /// </summary>
        protected IDbContextTransaction ContextTran => _contextTran ?? throw new ArgumentException("事务已释放");

        /// <summary>
        /// 工作单元事务
        /// </summary>
        /// <param name="contextTran"></param>
        public UnitOfWorkTransaction(IDbContextTransaction contextTran)
        {
            _contextTran = contextTran;
        }
        /// <summary>
        /// 当前事务
        /// </summary>
        public DbTransaction Transaction => ContextTran.GetDbTransaction();
        /// <summary>
        /// 事务ID
        /// </summary>
        public Guid TransactionId => ContextTran.TransactionId;
        /// <summary>
        /// 提交
        /// </summary>
        /// <returns></returns>
        public void Commit() => ContextTran.Commit();
        /// <summary>
        /// 回滚
        /// </summary>
        public void Rollback() => ContextTran.Rollback();
        /// <summary>
        /// 提交(异步)
        /// </summary>
        /// <returns></returns>
        public Task CommitAsync() => ContextTran.CommitAsync(TaskCancelOption.DbTask.Token);
        /// <summary>
        /// 回滚(异步)
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task RollbackAsync() => ContextTran.RollbackAsync(TaskCancelOption.DbTask.Token);
        /// <summary>
        /// 析构
        /// </summary>
        ~UnitOfWorkTransaction() => Dispose();
        /// <summary>
        /// 释放
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// 释放(异步)
        /// </summary>
        /// <returns></returns>
        public async ValueTask DisposeAsync()
        {
            await DisposeAsyncCore();
            Dispose(false);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// 实现释放资源
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Rollback();
                _contextTran?.Dispose();
            }
            _contextTran = default;
        }
        /// <summary>
        /// 实现异步释放资源
        /// </summary>
        /// <returns></returns>
        protected virtual async ValueTask DisposeAsyncCore()
        {
            if (_contextTran != default)
            {
                await RollbackAsync();
                await _contextTran.DisposeAsync().ConfigureAwait(false);
            }
            _contextTran = default;
        }
    }
}
