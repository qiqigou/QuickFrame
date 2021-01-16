using System.Threading.Tasks;
using QuickFrame.Common;

namespace QuickFrame.IServices
{
    /// <summary>
    /// 获取选项(下拉值选项)
    /// </summary>
    public interface IQueryOption
    {
        /// <summary>
        /// 获取选项
        /// </summary>
        /// <param name="tag">标签</param>
        /// <param name="page">分页</param>
        /// <returns></returns>
        Task<PageOutput<OptionOutput>> GetOptionsAsync(string tag, PageInput page);
        /// <summary>
        /// 根据模糊串查询选项
        /// </summary>
        /// <param name="tag">标签</param>
        /// <param name="page">分页</param>
        /// <param name="matchmsg">模糊串</param>
        /// <returns></returns>
        Task<PageOutput<OptionOutput>> GetOptionsAsync(string tag, PageInput page, string matchmsg);
    }
}
