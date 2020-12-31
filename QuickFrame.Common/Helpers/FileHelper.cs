using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace QuickFrame.Common
{
    public class FileHelper
    {
        /// <summary>
        /// 写文本文件
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="text">文件内容</param>
        public async static Task WriteTextFileAsync(string path, string text)
        {
            if (!File.Exists(path))
            {
                File.Create(path).Close();
            }
            await using var streamWriter = new StreamWriter(path, false);
            await streamWriter.WriteAsync(text);
        }
        /// <summary>
        /// 写文本文件
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="text">文件内容</param>
        /// <param name="encode">编码格式</param>
        public async static Task WriteTextFileAsync(string path, string text, Encoding encode)
        {
            if (!File.Exists(path))
            {
                File.Create(path).Close();
            }
            await using var streamWriter = new StreamWriter(path, false, encode);
            await streamWriter.WriteAsync(text);
        }
        /// <summary>
        /// 读文本文件
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns></returns>
        public async static Task<string> ReadTextFileAsync(string path)
        {
            if (!File.Exists(path)) throw new ArgumentException($"文件路径{nameof(path)}不存在");
            using var streamReader = new StreamReader(path);
            return await streamReader.ReadToEndAsync();
        }
        /// <summary>
        /// 读文本文件
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="encode">编码格式</param>
        /// <returns></returns>
        public async static Task<string> ReadTextFileAsync(string path, Encoding encode)
        {
            if (!File.Exists(path)) throw new ArgumentException($"文件路径{nameof(path)}不存在");
            using var streamReader = new StreamReader(path, encode);
            return await streamReader.ReadToEndAsync();
        }
    }
}
