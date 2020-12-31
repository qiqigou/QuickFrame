namespace QuickFrame.Common
{
    /// <summary>
    /// 消息码定义
    /// </summary>
    public static partial class MessageCodeOption
    {
        /// <summary>
        /// 成功
        /// </summary>
        public static MessageCode Success => new MessageCode(200, "成功");
        /// <summary>
        /// 失败
        /// </summary>
        public static MessageCode Bad => new MessageCode(400, "失败");
        /// <summary>
        /// 未登录
        /// </summary>
        public static MessageCode Unauthorized => new MessageCode(401, "未登录");
        /// <summary>
        /// 未授权
        /// </summary>
        public static MessageCode Forbidden => new MessageCode(403, "未授权");
        /// <summary>
        /// 审核失败
        /// </summary>
        public static MessageCode Audt => new MessageCode(410, "审核失败");
        /// <summary>
        /// 弃审失败
        /// </summary>
        public static MessageCode UnAudt => new MessageCode(411, "弃审失败");
        /// <summary>
        /// 审批失败
        /// </summary>
        public static MessageCode Approve => new MessageCode(412, "审批失败");
        /// <summary>
        /// 弃批失败
        /// </summary>
        public static MessageCode UnApprove => new MessageCode(413, "弃批失败");
        /// <summary>
        /// 任务超时
        /// </summary>
        public static MessageCode Cancel => new MessageCode(550, "任务超时");
        /// <summary>
        /// 内部错误(属于系统错误，程序BUG)
        /// </summary>
        public static MessageCode Error => new MessageCode(500, "内部错误");
    }
    /// <summary>
    /// 自定义消息码
    /// </summary>
    public partial class MessageCodeOption
    {
        /// <summary>
        /// 请求参数错误
        /// </summary>
        public static MessageCode Bad_001 => new MessageCode(400_001, Bad.Title, "请求参数错误");
        /// <summary>
        /// 查询条件语法错误.{0}
        /// </summary>
        public static MessageCode Bad_002 => new MessageCode(400_002, Bad.Title, "查询条件语法错误.{0}", new[] { "语法错误" });
        /// <summary>
        /// 数据{0}已被修改,请重试
        /// </summary>
        public static MessageCode Bad_Update => new MessageCode(400_003, Bad.Title, "数据{0}已被修改,请重试", new[] { "_" });
        /// <summary>
        /// 数据{0}不存在
        /// </summary>
        public static MessageCode Bad_Delete => new MessageCode(400_004, Bad.Title, "数据{0}不存在", new[] { "_" });
        /// <summary>
        /// {0}格式错误
        /// </summary>
        public static MessageCode Bad_Format => new MessageCode(400_005, Bad.Title, "{0}格式错误", new[] { "_" });
        /// <summary>
        /// Token已过期
        /// </summary>
        public static MessageCode Bad_Token => new MessageCode(400_006, Bad.Title, "Token已过期");
        /// <summary>
        /// Token无效
        /// </summary>
        public static MessageCode Bad_TokenInvalid => new MessageCode(400_007, Bad.Title, "Token无效");
    }
}
