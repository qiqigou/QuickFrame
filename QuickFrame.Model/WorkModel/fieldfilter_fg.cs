using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using QuickFrame.Common;

namespace QuickFrame.Model
{
    /// <summary>
    /// 字段过滤主表
    /// </summary>
    public class fieldfilter_fg : WithStampTable, IDbEntity<WorkOption>
    {
        /// <summary>
        /// id
        /// </summary>
        [Key]
        public long fg_id { get; set; }
        /// <summary>
        /// 源类型名称
        /// </summary>
        [StringColumn]
        public string fg_source { get; set; } = string.Empty;
        /// <summary>
        /// 目标类型名称
        /// </summary>
        [StringColumn]
        public string fg_dest { get; set; } = string.Empty;

        public IEnumerable<fieldfilterc_fgc>? fieldfilterc_fgc { get; set; }
    }
}
