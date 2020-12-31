using System;

namespace QuickFrame.Common
{
    /// <summary>
    /// 禁用操作日志(未实现)
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class NoOprationLogAttribute : Attribute
    {
    }
}
