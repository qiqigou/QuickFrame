using System;

namespace QuickFrame.Common
{
    /// <summary>
    /// 带时间戳的输入模型
    /// </summary>
    public abstract class WithStampDataInput : IDataInput
    {
        /// <summary>
        /// 时间戳
        /// </summary>
        public byte[] Timestamp { get; set; } = Array.Empty<byte>();
    }
}
