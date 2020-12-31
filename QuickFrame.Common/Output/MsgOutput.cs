namespace QuickFrame.Common
{
    /// <summary>
    /// 响应消息
    /// </summary>
    public class MsgOutput : IMEntity
    {
        /// <summary>
        /// 消息码
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; } = string.Empty;
        /// <summary>
        /// 消息详细列表
        /// </summary>
        public string[]? MsgDetail { get; set; }
        /// <summary>
        /// 构造成功消息
        /// </summary>
        public MsgOutput()
        {
            Code = MessageCodeOption.Success.Code;
            Title = MessageCodeOption.Success.Title;
        }
        /// <summary>
        /// 构造消息码消息
        /// </summary>
        /// <param name="messageCode"></param>
        public MsgOutput(MessageCode messageCode)
        {
            Code = messageCode.Code;
            Title = messageCode.Title;
            MsgDetail = messageCode.MsgTemplate != default ? new[] { messageCode.MsgTemplate } : default;
        }
    }
}
