using QuickFrame.Common;
using QuickFrame.Models;
using System.Threading.Tasks;

namespace QuickFrame.IServices
{
    /// <summary>
    /// 审批弃审批
    /// </summary>
    public interface IApprove<TEntity, TKey>
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
