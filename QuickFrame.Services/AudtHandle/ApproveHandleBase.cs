using QuickFrame.Models;
using System.Threading.Tasks;

namespace QuickFrame.Services
{
    /// <summary>
    /// 审批抽象类
    /// </summary>
    public abstract class ApproveHandleBase<TEntity, TKey> : IApproveHandle<TEntity, TKey>
        where TEntity : TableEntity, new()
        where TKey : notnull
    {
        protected abstract Task ApproveAsync(TKey key);

        public Task ApproveRangeAsync(TKey[] keys) => Task.CompletedTask;

        protected abstract Task UnApproveAsync(TKey key);

        public Task UnApproveRangeAsync(TKey[] keys) => Task.CompletedTask;

        protected abstract Task HandleAsync();
    }
}
