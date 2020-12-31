using System.Collections.Generic;

namespace QuickFrame.Common
{
    /// <summary>
    /// 分页信息输出
    /// </summary>
    public class PageOutput<TValue> : IMEntity
        where TValue : notnull
    {
        /// <summary>
        /// 数据总数
        /// </summary>
        public long Total { get; set; } = 0;
        /// <summary>
        /// 当前页码
        /// </summary>
        public int Index { get; set; } = 1;
        /// <summary>
        /// 每页条目
        /// </summary>
        public int Size { get; set; } = 20;
        /// <summary>
        /// 数据
        /// </summary>
        public IEnumerable<TValue>? DataList { get; set; }
    }

    public static class PageOutput
    {
        /// <summary>
        /// 分页信息转换
        /// </summary>
        /// <typeparam name="TIn"></typeparam>
        /// <typeparam name="TOut"></typeparam>
        /// <param name="page"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public static PageOutput<TOut> Convert<TIn, TOut>(PageOutput<TIn> page, IEnumerable<TOut>? list)
            where TIn : notnull
            where TOut : notnull
        {
            return new PageOutput<TOut>
            {
                Total = page.Total,
                Index = page.Index,
                Size = page.Size,
                DataList = list
            };
        }
        /// <summary>
        /// 分页信息转换
        /// </summary>
        /// <param name="total"></param>
        /// <param name="page"></param>
        /// <param name="list"></param>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        public static PageOutput<TValue> Convert<TValue>(int total, PageInput page, IEnumerable<TValue>? list)
            where TValue : notnull
        {
            return new PageOutput<TValue>
            {
                Total = total,
                Index = page.Index,
                Size = page.Size,
                DataList = list
            };
        }
    }
}
