using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using QuickFrame.Common;

namespace QuickFrame.Models
{
    /// <summary>
    /// 分类基础表
    /// </summary>
    public class sysclassset_cs : WithStampTable, IDbEntity<BackOption>
    {
        /// <summary>
        /// 分类基础ID
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int cs_iid { get; set; }
        /// <summary>
        /// 分类基础名称
        /// </summary>
        [StringColumn]
        public string cs_cname { get; set; } = string.Empty;
        /// <summary>
        /// 分类基础说明
        /// </summary>
        [StringColumn]
        public string cs_cremark { get; set; } = string.Empty;
        /// <summary>
        /// 分类
        /// </summary>
        public IEnumerable<sysclassdtl_cd>? sysuserlist_ul { get; set; }
    }
}
