using QuickFrame.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuickFrame.Models
{
    /// <summary>
    /// 字段过滤子表
    /// </summary>
    public class fieldfilterc_fgc : TableEntity, IDbEntity<WorkOption>
    {
        /// <summary>
        /// 主表外键
        /// </summary>
        [Key]
        public long fg_id { get; set; }
        /// <summary>
        /// 项次
        /// </summary>
        [Key]
        public int fgc_iorder { get; set; }
        /// <summary>
        /// 字段名
        /// </summary>
        [StringColumn]
        public string fgc_field { get; set; } = string.Empty;
        /// <summary>
        /// 主表
        /// </summary>
        [ForeignKey(nameof(fg_id))]
        public fieldfilter_fg? fieldfilter_fg { get; set; }
    }
}
