using QuickFrame.Common;
using QuickFrame.Models;
using System.Threading.Tasks;

namespace QuickFrame.Services
{
    /// <summary>
    /// 审批抽象类
    /// </summary>
    public abstract class ApproveHandleBase<TEntity, TKey> : IApproveHandle<TEntity, TKey>
        where TEntity : WithStampTable, new()
        where TKey : notnull
    {
        protected abstract Task ApproveAsync(AudtInput<TKey> input);

        public Task ApproveRangeAsync(AudtInput<TKey>[] inputs) => Task.CompletedTask;

        protected abstract Task UnApproveAsync(AudtInput<TKey> input);

        public Task UnApproveRangeAsync(AudtInput<TKey>[] inputs) => Task.CompletedTask;

        protected abstract Task HandleAsync();
    }
}
