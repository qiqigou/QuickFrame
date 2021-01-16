using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using QuickFrame.Common;

namespace QuickFrame.Models
{
    /// <summary>
    /// 商场表
    /// </summary>
    public class syscompanybak_scb : WithStampTable, IDbEntity<BackOption>
    {
        /// <summary>
        /// 商场ID
        /// </summary>
        [Key]
        public int scb_corder { get; set; }
        /// <summary>
        /// 公司编号
        /// </summary>
        [Key]
        [StringColumn]
        public string scy_ccompanyid { get; set; } = string.Empty;
        /// <summary>
        /// 商场编号
        /// </summary>
        [StringColumn]
        public string scs_cshopid { get; set; } = string.Empty;
        /// <summary>
        /// 备份时间
        /// </summary>
        public DateTime? scb_ddate { get; set; }
        /// <summary>
        /// 备份文件名
        /// </summary>
        [StringColumn]
        public string scb_ctype { get; set; } = string.Empty;
        /// <summary>
        /// 专卖店
        /// </summary>
        public IEnumerable<syscompanyshop_scs>? syscompanyshop_scs;
    }
}
