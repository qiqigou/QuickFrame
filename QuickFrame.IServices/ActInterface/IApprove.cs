using QuickFrame.Models;
using System.Threading.Tasks;

namespace QuickFrame.IServices
{
    /// <summary>
    /// 审批弃审批
    /// </summary>
    public interface IApprove<TEntity, TKey>
        where TEntity : TableEntity, new()
        where TKey : notnull
    {
        /// <summary>
        /// 审批
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        Task ApproveRangeAsync(TKey[] keys);
        /// <summary>
        /// 弃批
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        Task UnApproveRangeAsync(TKey[] keys);
    }
}
