namespace QuickFrame.Common
{
    /// <summary>
    /// 主键输出实体
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public class KeyOutput<TKey> : IMEntity
        where TKey : notnull
    {
        public TKey KeyValue { get; set; }

        public KeyOutput(TKey keyvalue)
        {
            KeyValue = keyvalue;
        }
    }
}
