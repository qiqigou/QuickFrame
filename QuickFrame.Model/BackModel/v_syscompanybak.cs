using System;

namespace QuickFrame.Model
{
    public class v_syscompanybak : WithStampView, IDbEntity<BackOption>
    {
        /// <summary>
        /// 商场ID
        /// </summary>
        public int scb_corder { get; set; }
        /// <summary>
        /// 公司编号
        /// </summary>
        public string scy_ccompanyid { get; set; } = string.Empty;
        /// <summary>
        /// 商场编号
        /// </summary>
        public string scs_cshopid { get; set; } = string.Empty;
        /// <summary>
        /// 备份时间
        /// </summary>
        public DateTime? scb_ddate { get; set; }
        /// <summary>
        /// 备份文件名
        /// </summary>
        public string scb_ctype { get; set; } = string.Empty;
    }
}
