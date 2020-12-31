using System;
using System.Collections.Generic;

namespace QuickFrame.Common
{
    /// <summary>
    /// 列举扩展
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// 指定字段去重
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            var seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }
        /// <summary>
        /// 遍历
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="sources"></param>
        /// <param name="func"></param>
        public static void ForEach<TSource>(this IEnumerable<TSource> sources, Action<TSource> func)
        {
            foreach (var item in sources)
            {
                func.Invoke(item);
            }
        }
        /// <summary>
        /// 遍历(带索引)
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="sources"></param>
        /// <param name="func"></param>
        public static void ForEach<TSource>(this IEnumerable<TSource> sources, Action<TSource, int> func)
        {
            var index = 0;
            foreach (var item in sources)
            {
                func.Invoke(item, index);
            }
        }
    }
}
