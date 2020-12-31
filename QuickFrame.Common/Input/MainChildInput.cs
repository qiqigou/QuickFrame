using System;
using System.Collections.Generic;

namespace QuickFrame.Common
{
    /// <summary>
    /// 主子表输入模型
    /// </summary>
    public class MainChildInput<TMaiTMainInputn, TChildInput> : IDataInput
        where TMaiTMainInputn : IDataInput, new()
        where TChildInput : IDataInput, new()
    {
        /// <summary>
        /// 主表
        /// </summary>
        public TMaiTMainInputn Main { get; set; } = new TMaiTMainInputn();
        /// <summary>
        /// 子表
        /// </summary>
        public IEnumerable<TChildInput> Child { get; set; } = Array.Empty<TChildInput>();
    }
}
