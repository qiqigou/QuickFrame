using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using QuickFrame.Common;

namespace QuickFrame.Models
{
    /// <summary>
    /// 公司表
    /// </summary>
    public class syscompany_scy : WithStampTable, IDbEntity<BackOption>
    {
        /// <summary>
        /// 公司编号
        /// </summary>
        [Key]
        [StringColumn]
        public string scy_ccompanyid { get; set; } = string.Empty;
        /// <summary>
        /// 公司名称
        /// </summary>
        [StringColumn]
        public string scy_ccompanyname { get; set; } = string.Empty;
        /// <summary>
        /// 公司负责人
        /// </summary>
        [StringColumn]
        public string scy_cman { get; set; } = string.Empty;
        /// <summary>
        /// 公司地址
        /// </summary>
        [StringColumn]
        public string scy_cadr { get; set; } = string.Empty;
        /// <summary>
        /// 公司电话
        /// </summary>
        [StringColumn]
        public string scy_ctel { get; set; } = string.Empty;
        /// <summary>
        /// 公司传真
        /// </summary>
        [StringColumn]
        public string scy_cfax { get; set; } = string.Empty;
        /// <summary>
        /// 公司邮箱
        /// </summary>
        [StringColumn]
        public string scy_cemail { get; set; } = string.Empty;
        /// <summary>
        /// 状态(1有效,0失效)
        /// </summary>
        [FlagColumn]
        public string scy_cstate { get; set; } = string.Empty;
        /// <summary>
        /// 公司网站
        /// </summary>
        [StringColumn]
        public string scy_cweb { get; set; } = string.Empty;
        /// <summary>
        /// 专卖店
        /// </summary>
        public IEnumerable<syscompanyshop_scs>? syscompanyshop_scs { get; set; }

    }
}
