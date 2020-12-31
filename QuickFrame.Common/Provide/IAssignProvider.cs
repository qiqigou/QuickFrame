using ObjAssignMap;

namespace QuickFrame.Common
{
    /// <summary>
    /// 赋值器提供者
    /// </summary>
    public interface IAssignProvider
    {
        /// <summary>
        /// 构建赋值器
        /// </summary>
        /// <returns></returns>
        IObjAssign CreateAssign();
    }
}
