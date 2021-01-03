using System.Collections.Generic;

namespace System.Runtime.CompilerServices
{
    /// <summary>
    /// 元组扩展
    /// </summary>
    public static class TupleExtensions
    {
        /// <summary>
        /// 转为array
        /// </summary>
        /// <param name="tuple"></param>
        /// <returns></returns>
        public static object?[] ToArray(this ITuple tuple)
        {
            var array = new object?[tuple.Length];
            for (int i = 0; i < tuple.Length; i++)
            {
                array[i] = tuple[i];
            }
            return array;
        }
        /// <summary>
        /// 转为list
        /// </summary>
        /// <param name="tuple"></param>
        /// <returns></returns>
        public static List<object?> ToList(this ITuple tuple)
        {
            var array = new List<object?>(tuple.Length);
            for (int i = 0; i < tuple.Length; i++)
            {
                array[i] = tuple[i];
            }
            return array;
        }
    }
}
