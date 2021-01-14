using QuickFrame.Common;
using System.Threading.Tasks;

namespace QuickFrame.IServices
{
    /// <summary>
    /// 主子表新增,修改,删除
    /// </summary>
    /// <typeparam name="TMainInput">新增主表时输入模型</typeparam>
    /// <typeparam name="TMainUpdInput">修改主表时输入模型</typeparam>
    /// <typeparam name="TChildInput">子表新增时输入模型</typeparam>
    /// <typeparam name="TChildUpdInput">子表修改时输入模型</typeparam>
    /// <typeparam name="TKey">主表主键</typeparam>
    public interface IHandleMainChild<TMainInput, TMainUpdInput, TChildInput, TChildUpdInput, TKey>
        where TMainInput : IDataInput, new()
        where TMainUpdInput : IDataInput, new()
        where TChildInput : IDataInput, new()
        where TChildUpdInput : IDataInput, new()
        where TKey : notnull
    {
        /// <summary>
        /// 新增主子表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<TKey> CreateMainChildAsync(MainChildInput<TMainInput, TChildInput> input);
        /// <summary>
        /// 修改主子表
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="timestamp"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<int> UpdateMainChildAsync(TKey keyValue, byte[] timestamp, MainChildInput<TMainUpdInput, TChildUpdInput> input);
        /// <summary>
        /// 根据主键删除主子表(支持批量)
        /// </summary>
        /// <param name="arrayKeyStamp"></param>
        /// <returns></returns>
        Task<int> DeleteMainChildAsync(KeyStamp<TKey>[] arrayKeyStamp);
    }
}
