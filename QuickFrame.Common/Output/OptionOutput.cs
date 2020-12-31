﻿namespace QuickFrame.Common
{
    /// <summary>
    /// 下拉选项输出
    /// </summary>
    public class OptionOutput<TValue> : IMEntity
        where TValue : notnull
    {
        /// <summary>
        /// 显示名
        /// </summary>
        public string Label { get; set; } = string.Empty;
        /// <summary>
        /// 真实值
        /// </summary>
        public TValue Value { get; set; } = default!;
        /// <summary>
        /// 禁用
        /// </summary>
        public bool Disabled { get; set; }
    }
}
