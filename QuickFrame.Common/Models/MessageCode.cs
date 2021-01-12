namespace QuickFrame.Common
{
    /// <summary>
    /// 消息码记录
    /// </summary>
    public record MessageCode : IMEntity
    {
        /// <summary>
        /// 消息码
        /// </summary>
        public int Code { get; init; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; init; }
        /// <summary>
        /// 消息模板
        /// </summary>
        public string? MsgTemplate { get; init; }
        /// <summary>
        /// 消息参数
        /// </summary>
        public object[]? MsgArgs { get; init; }
        /// <summary>
        /// 消息内容
        /// </summary>
        public string Message
        {
            get
            {
                return MsgArgs?.Length > 0 ? string.Format(MsgTemplate ?? string.Empty, MsgArgs) : MsgTemplate ?? string.Empty;
            }
        }

        public MessageCode(int code, string title, string? msgtemplate = default, object[]? msgargs = default)
        {
            Code = code;
            Title = title;
            MsgTemplate = msgtemplate;
            MsgArgs = msgargs;
        }
    }
}
