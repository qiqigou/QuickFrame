using System;

namespace QuickFrame.Common
{
    /// <summary>
    /// 单例注入
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class SingletonInjectionAttribute : Attribute
    {
    }
}
