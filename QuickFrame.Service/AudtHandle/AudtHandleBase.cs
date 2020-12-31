using System.Threading.Tasks;
using QuickFrame.Common;
using QuickFrame.Model;

namespace QuickFrame.Service
{
    /// <summary>
    /// 审核抽象类
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public abstract class AudtHandleBase<TEntity, TKey> : IAudtHandle<TEntity, TKey>
        where TEntity : WithStampTable, new()
        where TKey : notnull
    {
        protected readonly string code = string.Empty;
        protected readonly string flag = ConstantOptions.DbFlag.DbTrue;
        protected readonly string dbtrue = ConstantOptions.DbFlag.DbTrue;
        protected readonly string dbfalse = ConstantOptions.DbFlag.DbFalse;
        protected string Option => flag == dbtrue ? AssignOption.Auth : AssignOption.UnAuth;
        /// <summary>
        /// 数量统一控制.
        /// 适用于审核弃审操作中具有相反加减操作的情况
        /// </summary>
        /// <param name="mqty"></param>
        /// <returns></returns>
        protected decimal ConvertQty(decimal mqty)
        {
            return flag == dbtrue ? mqty : -mqty;
        }
        /// <summary>
        /// 通用检查
        /// </summary>
        protected virtual Task CommonQueryCheckedAsync() => Task.CompletedTask;
        /// <summary>
        /// 审核,
        /// 查询和检查
        /// </summary>
        protected virtual Task AudtQueryCheckedAsync() => Task.CompletedTask;
        /// <summary>
        /// 弃审,
        /// 查询和检查
        /// </summary>
        protected virtual Task UnAudtQueryCheckedAsync() => Task.CompletedTask;
        /// <summary>
        /// 审批,
        /// 查询和检查
        /// </summary>
        protected virtual Task ApproveQueryCheckedAsync() => Task.CompletedTask;
        /// <summary>
        /// 反审批,
        /// 查询和检查
        /// </summary>
        protected virtual Task UnApproveQueryCheckedAsync() => Task.CompletedTask;
        /// <summary>
        /// 审核\弃审
        /// </summary>
        protected abstract Task HandleAsync();
        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected abstract Task AudtAsync(AudtInput<TKey> input);
        /// <summary>
        /// 审核(集合)
        /// </summary>
        /// <param name="inputs"></param>
        /// <returns></returns>
        public Task AudtRangeAsync(AudtInput<TKey>[] inputs) => Task.CompletedTask;
        /// <summary>
        /// 弃审
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected abstract Task UnAudtAsync(AudtInput<TKey> input);
        /// <summary>
        /// 弃审(集合)
        /// </summary>
        /// <param name="inputs"></param>
        /// <returns></returns>
        public Task UnAudtRangeAsync(AudtInput<TKey>[] inputs) => Task.CompletedTask;
    }
}
