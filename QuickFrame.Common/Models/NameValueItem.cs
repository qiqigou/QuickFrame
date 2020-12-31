namespace QuickFrame.Common
{
    /// <summary>
    /// 名称和值模型
    /// </summary>
    public class NameValueItem : IMEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
    }
}
