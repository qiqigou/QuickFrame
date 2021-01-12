using QuickFrame.Models;
using System.Threading.Tasks;

namespace QuickFrame.Services
{
    public interface IApproveHandle<TEntity, TKey>
        where TEntity : TableEntity, new()
        where TKey : notnull
    {
        /// <summary>
        /// 审批
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task ApproveRangeAsync(TKey[] key);
        /// <summary>
        /// 弃批
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        Task UnApproveRangeAsync(TKey[] keys);
    }
}
