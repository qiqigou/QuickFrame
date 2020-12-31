namespace QuickFrame.Common
{
    /// <summary>
    /// 下拉选项输出
    /// </summary>
    public class OptionOutput : IMEntity
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Label { get; set; } = string.Empty;
        /// <summary>
        /// 值
        /// </summary>
        public object? Value { get; set; }
        /// <summary>
        /// 禁用
        /// </summary>
        public bool Disabled { get; set; }
        /// <summary>
        /// 额外数据
        /// </summary>
        public object? Data { get; set; }
    }
}
