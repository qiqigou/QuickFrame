namespace QuickFrame.Common
{
    /// <summary>
    /// 日志事件ID
    /// </summary>
    public static class LogEventsOption
    {
        /// <summary>
        /// 创建项
        /// </summary>
        public const int GenerateItems = 1000;
        /// <summary>
        /// 获取列表
        /// </summary>
        public const int ListItems = 1001;
        /// <summary>
        /// 获取项
        /// </summary>
        public const int GetItem = 1002;
        /// <summary>
        /// 新增项
        /// </summary>
        public const int InsertItem = 1003;
        /// <summary>
        /// 修改项
        /// </summary>
        public const int UpdateItem = 1004;
        /// <summary>
        /// 删除项
        /// </summary>
        public const int DeleteItem = 1005;
        /// <summary>
        /// 获取空项
        /// </summary>
        public const int GetItemNotFound = 1006;
        /// <summary>
        /// 修改空项
        /// </summary>
        public const int UpdateItemNotFound = 1007;
        /// <summary>
        /// 任务取消(任务超时)
        /// </summary>
        public const int Cancel = 3000;
        /// <summary>
        /// 业务错误
        /// </summary>
        public const int Bad = 4000;
        /// <summary>
        /// 系统错误
        /// </summary>
        public const int Error = 5000;
    }
}
