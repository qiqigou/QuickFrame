using System.Threading.Tasks;
using QuickFrame.Models;

namespace QuickFrame.Services
{
    public interface IAudtHandle<TEntity, TKey>
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
