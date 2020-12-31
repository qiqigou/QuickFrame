namespace QuickFrame.Common
{
    /// <summary>
    /// 名称和值模型
    /// </summary>
    public class NameValueItem<TValue> : IMEntity
        where TValue : notnull
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// 值
        /// </summary>
        public TValue Value { get; set; } = default!;
    }
}
