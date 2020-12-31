using System.Threading.Tasks;
using QuickFrame.Common;

namespace QuickFrame.Service
{
    /// <summary>
    /// 获取选项(下拉值选项)
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IQueryOption<TEntity>
        where TEntity : IMEntity, new()
    {
        /// <summary>
        /// 获取选项
        /// </summary>
        /// <returns></returns>
        Task<PageOutput<TEntity>> GetOptionsAsync(PageInput? input = default);
        /// <summary>
        /// 根据模糊串获取选项
        /// </summary>
        /// <param name="matchmsg"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PageOutput<TEntity>> GetOptionsAsync(string matchmsg, PageInput? input = default);
    }
}
