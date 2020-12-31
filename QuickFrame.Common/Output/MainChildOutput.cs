using System;
using System.Collections.Generic;

namespace QuickFrame.Common
{
    /// <summary>
    /// 主子表合并输出
    /// </summary>
    /// <typeparam name="TMain"></typeparam>
    /// <typeparam name="TChild"></typeparam>
    public class MainChildOutput<TMain, TChild> : IMEntity
        where TMain : IMEntity, new()
        where TChild : IMEntity, new()
    {
        /// <summary>
        /// 主表
        /// </summary>
        public TMain Main { get; set; } = new TMain();
        /// <summary>
        /// 子表
        /// </summary>
        public IEnumerable<TChild> Child { get; set; } = Array.Empty<TChild>();
    }
}
