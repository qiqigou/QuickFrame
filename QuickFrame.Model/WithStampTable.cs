using System;
using System.ComponentModel.DataAnnotations;

namespace QuickFrame.Models
{
    /// <summary>
    /// 表(带时间戳)
    /// </summary>
    public abstract class WithStampTable : TableEntity
    {
        /// <summary>
        /// 时间戳
        /// </summary>
        [Timestamp]
        public byte[] timestamp { get; set; } = Array.Empty<byte>();
    }
}
