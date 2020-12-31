using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuickFrame.Common
{
    /// <summary>
    /// 数据类型转换
    /// </summary>
    public static class UtilConvert
    {
        /// <summary>
        /// 转换为16进制
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="lowerCase">是否小写</param>
        /// <returns></returns>
        public static string? ToHex(this byte[]? bytes, bool lowerCase = true)
        {
            if (bytes == default) return default;
            var result = new StringBuilder();
            var format = lowerCase ? "x2" : "X2";
            for (var i = 0; i < bytes.Length; i++)
            {
                result.Append(bytes[i].ToString(format));
            }
            return result.ToString();
        }
        /// <summary>
        /// 16进制转字节数组
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static byte[]? HexToBytes(this string? s)
        {
            if (s?.IsNull() ?? true) return default;
            var bytes = new byte[s.Length / 2];
            for (int x = 0; x < s.Length / 2; x++)
            {
                int i = (Convert.ToInt32(s.Substring(x * 2, 2), 16));
                bytes[x] = (byte)i;
            }
            return bytes;
        }
        /// <summary>
        /// 转换为Base64
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string? ToBase64(this byte[]? bytes)
        {
            if (bytes == default) return default;
            return Convert.ToBase64String(bytes);
        }
        /// <summary>
        /// 去除空值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <returns></returns>
        public static IEnumerable<T>? RemoveNull<T>(this IEnumerable<T>? array)
            where T : class
        {
            if (array == default) goto end;
            foreach (var item in array)
            {
                if (item != default) yield return item;
            }
        end:;
        }
        /// <summary>
        /// 去除空值
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="array"></param>
        /// <returns></returns>
        public static TValue[]? RemoveNull<TValue>(this TValue[]? array) => array?.Where(x => x != null)?.ToArray();
        /// <summary>
        /// 获取枚举的Options
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <returns></returns>
        public static IEnumerable<OptionOutput<int>> GetOptions<TEnum>()
            where TEnum : Enum
        {
            var enumType = typeof(TEnum);
            if (!enumType.IsEnum) Array.Empty<OptionOutput<int>>();
            return Enum.GetValues(enumType).Cast<Enum>().Select(x => new OptionOutput<int>
            {
                Label = x.GetDescription(),
                Value = Convert.ToInt32(x)
            }).ToArray();
        }
    }
}
