using System;
using System.Collections.Generic;

namespace QuickFrame.Common
{
    /// <summary>
    /// 普通下拉选项输出
    /// </summary>
    public class OptionOutput : IMEntity
    {
        /// <summary>
        /// 显示名
        /// </summary>
        public string Label { get; set; } = string.Empty;
        /// <summary>
        /// 真实值
        /// </summary>
        public object Value { get; set; } = new();
        /// <summary>
        /// 禁用
        /// </summary>
        public bool Disabled { get; set; }
    }
    /// <summary>
    /// 级联下拉值选项输出
    /// </summary>
    public class CascaderOptionOutput : OptionOutput
    {
        /// <summary>
        /// 子集
        /// </summary>
        public IEnumerable<OptionOutput> Children { get; set; } = Array.Empty<OptionOutput>();
    }
}
