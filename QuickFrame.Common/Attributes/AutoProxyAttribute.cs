using System;
using System.Collections.Generic;

namespace QuickFrame.Common
{
    /// <summary>
    /// 设置代理(AOP)
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class AutoProxyAttribute : Attribute
    {
        private readonly string[] _options;
        public IReadOnlyList<string> Options => _options;

        /// <summary>
        /// 设置代理(AOP)
        /// </summary>
        /// <param name="options"></param>
        public AutoProxyAttribute(params string[] options)
        {
            _options = options;
        }
    }
}
