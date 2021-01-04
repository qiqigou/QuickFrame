namespace QuickFrame.Models
{
    /// <summary>
    /// 商场-专卖店视图
    /// </summary>
    public class v_syscompanyshop : WithStampView, IDbEntity<BackOption>
    {
        /// <summary>
        /// 专卖店编码
        /// </summary>
        public string scs_cshopid { get; set; } = string.Empty;
        /// <summary>
        /// 专卖店名称
        /// </summary>
        public string scs_cshopname { get; set; } = string.Empty;
        /// <summary>
        /// 公司编码
        /// </summary>
        public string scy_ccompanyid { get; set; } = string.Empty;
        /// <summary>
        /// 公司名称
        /// </summary>
        public string scy_ccompanyname { get; set; } = string.Empty;
        /// <summary>
        /// 标志
        /// </summary>
        public string scs_cmcflag { get; set; } = string.Empty;
        /// <summary>
        /// 账套名称
        /// </summary>
        public string scs_cdbname { get; set; } = string.Empty;

    }
}
