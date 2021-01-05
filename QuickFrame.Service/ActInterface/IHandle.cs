using QuickFrame.Common;
using System.Threading.Tasks;

namespace QuickFrame.Service
{
    /// <summary>
    /// 新增,修改,删除
    /// </summary>
    public interface IHandle<TInput, TUpdInput, TKey>
        where TInput : IDataInput, new()
        where TUpdInput : WithStampDataInput, new()
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
        /// <param name="input">数据</param>
        /// <returns></returns>
        Task<int> UpdateAsync(TKey keyValue, TUpdInput input);
        /// <summary>
        /// 删除(根据主键)
        /// </summary>
        /// <param name="arrayKeyValue"></param>
        /// <returns></returns>
        Task<int> DeleteAsync(TKey[] arrayKeyValue);
    }
}
