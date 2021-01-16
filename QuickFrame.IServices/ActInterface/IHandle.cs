using System.Threading.Tasks;
using QuickFrame.Common;

namespace QuickFrame.IServices
{
    /// <summary>
    /// 新增,修改,删除
    /// </summary>
    public interface IHandle<TInput, TUpdInput, TKey>
        where TInput : IDataInput, new()
        where TUpdInput : IDataInput, new()
        where TKey : notnull
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<TKey> CreateAsync(TInput input);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <param name="timestamp"></param>
        /// <param name="input">数据</param>
        /// <returns></returns>
        Task<int> UpdateAsync(TKey keyValue, byte[] timestamp, TUpdInput input);
        /// <summary>
        /// 删除(根据主键)
        /// </summary>
        /// <param name="arrayKeyStamp"></param>
        /// <returns></returns>
        Task<int> DeleteAsync(KeyStamp<TKey>[] arrayKeyStamp);
    }
}
