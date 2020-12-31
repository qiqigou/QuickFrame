using System.Threading;

namespace QuickFrame.Common
{
    /// <summary>
    /// 任务取消配置选项
    /// </summary>
    public static class TaskCancelOption
    {
        /// <summary>
        /// 简单任务(超时时间:3秒)
        /// </summary>
        public static CancellationTokenSource SimpleTask => new CancellationTokenSource(3_000);
        /// <summary>
        /// 复杂任务(超时时间:10秒)
        /// </summary>
        public static CancellationTokenSource ComplexTask => new CancellationTokenSource(10_000);
        /// <summary>
        /// Db任务(超时时间:20秒)
        /// </summary>
        /// <returns></returns>
        public static CancellationTokenSource DbTask => new CancellationTokenSource(20_000);
    }
}
