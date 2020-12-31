using System;

namespace QuickFrame.Repository
{
    /// <summary>
    /// 定义账套库SQL脚本的生成行为
    /// </summary>
    public interface ISQLScriptRepository : IRepository
    {
        /// <summary>
        /// 生成SQL脚本
        /// </summary>
        /// <returns></returns>
        string GenerateCreateScript();
    }
}
