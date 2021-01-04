namespace QuickFrame.Models
{
    public class v_syscompany : WithStampView, IDbEntity<BackOption>
    {
        /// <summary>
        /// 公司编号
        /// </summary>
        public string scy_ccompanyid { get; set; } = string.Empty;
        /// <summary>
        /// 公司名称
        /// </summary>
        public string scy_ccompanyname { get; set; } = string.Empty;
        /// <summary>
        /// 公司负责人
        /// </summary>
        public string scy_cman { get; set; } = string.Empty;
        /// <summary>
        /// 公司地址
        /// </summary>
        public string scy_cadr { get; set; } = string.Empty;
        /// <summary>
        /// 公司电话
        /// </summary>
        public string scy_ctel { get; set; } = string.Empty;
        /// <summary>
        /// 公司传真
        /// </summary>
        public string scy_cfax { get; set; } = string.Empty;
        /// <summary>
        /// 公司邮箱
        /// </summary>
        public string scy_cemail { get; set; } = string.Empty;
        /// <summary>
        /// 状态(1有效,0失效)
        /// </summary>
        public string scy_cstate { get; set; } = string.Empty;
        /// <summary>
        /// 公司网站
        /// </summary>
        public string scy_cweb { get; set; } = string.Empty;
    }
}
