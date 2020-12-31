using System;

namespace QuickFrame.Common
{
    /// <summary>
    /// 输出到控制台
    /// </summary>
    public static class ConsoleHelper
    {
        private static void WriteColorLine(string str, ConsoleColor color)
        {
            ConsoleColor currentForeColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(str);
            Console.ForegroundColor = currentForeColor;
        }
        /// <summary>
        /// 打印错误信息
        /// </summary>
        /// <param name="str">待打印的字符串</param>
        public static void WriteErrorLine(this string str)
        {
            WriteColorLine(str, ConsoleColor.Red);
        }
        /// <summary>
        /// 打印警告信息
        /// </summary>
        /// <param name="str">待打印的字符串</param>
        public static void WriteWarningLine(this string str)
        {
            WriteColorLine(str, ConsoleColor.Yellow);
        }
        /// <summary>
        /// 打印正常信息
        /// </summary>
        /// <param name="str">待打印的字符串</param>
        public static void WriteInfoLine(this string str)
        {
            WriteColorLine(str, ConsoleColor.White);
        }
        /// <summary>
        /// 打印成功的信息
        /// </summary>
        /// <param name="str">待打印的字符串</param>
        public static void WriteSuccessLine(this string str)
        {
            WriteColorLine(str, ConsoleColor.Green);
        }
    }
}
