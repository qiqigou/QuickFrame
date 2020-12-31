namespace QuickFrame.Common
{
    /// <summary>
    /// 查询时过滤条件
    /// </summary>
    public class SQLFilterInput : IDataInput
    {
        /// <summary>
        /// 查询条件
        /// </summary>
        [TextColumn]
        public string Condition { get; set; } = string.Empty;
        /// <summary>
        /// 分页信息
        /// </summary>
        public PageInput Page { get; set; } = new PageInput();
    }
}
