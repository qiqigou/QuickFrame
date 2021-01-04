using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuickFrame.Services
{
    /// <summary>
    /// 布局服务(提供列字段,描述,主键等信息)
    /// </summary>
    public interface ILayoutService : IService
    {
        /// <summary>
        /// 查询视图字段名和描述
        /// </summary>
        /// <param name="viewName">视图名称</param>
        /// <returns></returns>
        Task<IEnumerable<ColumnItemOutput>> QueryViewColumnsAsync(string viewName);
        /// <summary>
        /// 获取全部视图
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<ViewItemOutput>> QueryViewsAsync();
    }
}
