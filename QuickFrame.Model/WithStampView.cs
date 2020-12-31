using System;
using System.ComponentModel.DataAnnotations;

namespace QuickFrame.Model
{
    /// <summary>
    /// 视图(带时间戳)
    /// </summary>
    public abstract class WithStampView : ViewEntity
    {
        /// <summary>
        /// 时间戳
        /// </summary>
        [Timestamp]
        public byte[] timestamp { get; set; } = Array.Empty<byte>();
    }
}
