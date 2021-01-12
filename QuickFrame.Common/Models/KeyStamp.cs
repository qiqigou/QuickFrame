namespace QuickFrame.Common
{
    /// <summary>
    /// 主键时间戳模型
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public class KeyStamp<TKey> : IMEntity
        where TKey : notnull
    {
        /// <summary>
        /// 主键
        /// </summary>
        public TKey Key { get; set; }
        /// <summary>
        /// 时间戳
        /// </summary>
        public byte[] Timpstamp { get; set; }

        public KeyStamp(TKey key, byte[] timestamp)
        {
            Key = key;
            Timpstamp = timestamp;
        }
    }
}
