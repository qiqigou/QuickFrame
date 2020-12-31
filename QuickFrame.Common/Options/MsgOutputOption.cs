namespace QuickFrame.Common
{
    /// <summary>
    /// 预设响应消息
    /// </summary>
    public class MsgOutputOption
    {
        /// <summary>
        /// 成功消息
        /// </summary>
        public static MsgOutput OkMsg => new MsgOutput(MessageCodeOption.Success);
        /// <summary>
        /// 失败消息
        /// </summary>
        public static MsgOutput BadMsg => new MsgOutput(MessageCodeOption.Bad);
        /// <summary>
        /// 内部错误
        /// </summary>
        public static MsgOutput ErrorMsg => new MsgOutput(MessageCodeOption.Error);
    }
}
