using System;

namespace QuickFrame.Common
{
    /// <summary>
    /// 范围内注入(请求内单实例)
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class ScopeInjectionAttribute : Attribute
    {
    }
}
