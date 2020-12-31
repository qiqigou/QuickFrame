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
        /// 详细信息
        /// </summary>
        public string? MsgDetail { get; init; }
        /// <summary>
        /// 消息参数
        /// </summary>
        public object[]? MsgArgs { get; init; }

        public MessageCode(int code, string title, string? msgdetail = default, object[]? msgargs = default)
        {
            Code = code;
            Title = title;
            MsgDetail = msgdetail;
            MsgArgs = msgargs;
        }
    }
}
