using System.Threading.Tasks;
using QuickFrame.Models;

namespace QuickFrame.IServices
{
    /// <summary>
    /// 审核弃审
    /// </summary>
    public interface IAudt<TEntity, TKey>
        where TEntity : TableEntity, new()
        where TKey : notnull
    {
        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        Task AudtRangeAsync(TKey[] keys);
        /// <summary>
        /// 弃审
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        Task UnAudtRangeAsync(TKey[] keys);
    }
}
