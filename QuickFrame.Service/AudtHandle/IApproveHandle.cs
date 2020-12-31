using System.Threading.Tasks;
using QuickFrame.Common;
using QuickFrame.Model;

namespace QuickFrame.Service
{
    public interface IApproveHandle<TEntity, TKey>
        where TEntity : WithStampTable, new()
        where TKey : notnull
    {
        /// <summary>
        /// 审批
        /// </summary>
        /// <param name="inputs"></param>
        /// <returns></returns>
        Task ApproveRangeAsync(AudtInput<TKey>[] inputs);
        /// <summary>
        /// 弃批
        /// </summary>
        /// <param name="inputs"></param>
        /// <returns></returns>
        Task UnApproveRangeAsync(AudtInput<TKey>[] inputs);
    }
}
