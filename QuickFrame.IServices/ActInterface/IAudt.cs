using QuickFrame.Common;
using QuickFrame.Models;
using System.Threading.Tasks;

namespace QuickFrame.IServices
{
    /// <summary>
    /// 审核弃审
    /// </summary>
    public interface IAudt<TEntity, TKey>
        where TEntity : WithStampTable, new()
        where TKey : notnull
    {
        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="inputs"></param>
        /// <returns></returns>
        Task AudtRangeAsync(AudtInput<TKey>[] inputs);
        /// <summary>
        /// 弃审
        /// </summary>
        /// <param name="inputs"></param>
        /// <returns></returns>
        Task UnAudtRangeAsync(AudtInput<TKey>[] inputs);
    }
}
