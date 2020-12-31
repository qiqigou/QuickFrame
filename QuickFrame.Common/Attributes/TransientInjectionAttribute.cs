using System;

namespace QuickFrame.Common
{
    /// <summary>
    /// 瞬态注入(每次都是新的对象)
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class TransientInjectionAttribute : Attribute
    {
    }
}
