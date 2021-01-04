using QuickFrame.Common;
using System.Threading.Tasks;

namespace QuickFrame.Services
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
        where TMainUpdInput : WithStampDataInput, new()
        where TChildInput : IDataInput, new()
        where TChildUpdInput : IDataInput, new()
        where TKey : notnull
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<TKey> CreateMainChildAsync(MainChildInput<TMainInput, TChildInput> input);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<int> UpdateMainChildAsync(TKey keyValue, MainChildInput<TMainUpdInput, TChildUpdInput> input);
        /// <summary>
        /// 根据主键删除(支持批量)
        /// </summary>
        /// <param name="arrayKeyValue"></param>
        /// <returns></returns>
        Task<int> DeleteMainChildAsync(TKey[] arrayKeyValue);
    }
}
