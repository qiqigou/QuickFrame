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
        public string Name { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public TValue Value { get; set; }

        public NameValueItem(string name, TValue value)
        {
            Name = name;
            Value = value;
        }
    }
}
