using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using QuickFrame.Common;

namespace QuickFrame.Models
{
    /// <summary>
    /// 分类表
    /// </summary>
    public class sysclassdtl_cd : WithStampTable, IDbEntity<BackOption>
    {
        /// <summary>
        /// 分类属性ID
        /// </summary>
        [Key]
        public int cs_iid { get; set; }
        /// <summary>
        /// 分类编码
        /// </summary>
        [Key]
        [StringColumn]
        public string cd_cclassid { get; set; } = string.Empty;
        /// <summary>
        /// 备注
        /// </summary>
        [StringColumn]
        public string cd_cremark { get; set; } = string.Empty;
        /// <summary>
        /// 分类名称
        /// </summary>
        [StringColumn]
        public string cd_cname { get; set; } = string.Empty;
        /// <summary>
        /// 层次码
        /// </summary>
        [StringColumn]
        public string cd_clevel { get; set; } = string.Empty;
        /// <summary>
        /// 图片路径
        /// </summary>
        [PathColumn]
        public string cd_cmainmap { get; set; } = string.Empty;
        /// <summary>
        /// 停用标识
        /// </summary>
        [FlagColumn]
        public string cd_cstop { get; set; } = string.Empty;
        /// <summary>
        /// 类别
        /// </summary>
        [ForeignKey(nameof(cs_iid))]
        public sysclassset_cs? sysclassset_cs { get; set; }
        /// <summary>
        /// 用户
        /// </summary>
        public IEnumerable<sysuserlist_ul>? sysuserlist_ul { get; set; }

    }
}
