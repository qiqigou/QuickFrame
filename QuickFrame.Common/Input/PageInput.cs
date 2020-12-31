namespace QuickFrame.Common
{
    /// <summary>
    /// 分页信息模型
    /// </summary>
    public partial class PageInput : IDataInput
    {
        /// <summary>
        /// 当前页码
        /// </summary>
        public int Index { get; set; } = 1;
        /// <summary>
        /// 每页条目
        /// </summary>
        public int Size { set; get; } = 20;
        /// <summary>
        /// 排序字段
        /// </summary>
        public SortInput[]? Sort { get; set; }
    }
    /// <summary>
    /// 排序信息模型
    /// </summary>
    public class SortInput : IDataInput
    {
        /// <summary>
        /// 排序字段
        /// </summary>
        [StringColumn]
        public string OrderBy { get; set; } = string.Empty;
        /// <summary>
        /// 是否降序(默认false)
        /// </summary>
        public bool Desc { get; set; }
    }
    /// <summary>
    /// 分页输入模型转换
    /// </summary>
    partial class PageInput
    {
        /// <summary>
        /// 分页输入模型处理
        /// </summary>
        /// <param name="total">条目总数</param>
        /// <param name="page">分页输入模型</param>
        /// <param name="defaultOrder">默认排序字段</param>
        /// <returns></returns>
        public static PageInput Convert(int total, PageInput? page, SortInput[] defaultOrder)
        {
            var defPage = new PageInput();
            if (page != default)
            {
                page.Size = page.Size < 1 ? defPage.Size : page.Size;
                int maxIndex = total > 0 ? total % page.Size == 0 ? total / page.Size : total / page.Size + 1 : 1;
                page.Index = page.Index > maxIndex ? maxIndex : page.Index < 1 ? 1 : page.Index;
                if (page.Sort is not null && page.Sort.Length > 0)
                {
                    return page;
                }
                defPage.Size = page.Size;
                defPage.Index = page.Index;
            }
            defPage.Sort = defaultOrder;
            return defPage;
        }
    }
}
