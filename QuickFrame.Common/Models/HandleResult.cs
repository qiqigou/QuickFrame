namespace QuickFrame.Common
{
    /// <summary>
    /// 返回值记录
    /// </summary>
    public record HandleResult<TValue>
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; init; } = true;
        /// <summary>
        /// 值
        /// </summary>
        public TValue? Value { get; init; }
        /// <summary>
        /// 消息码集合
        /// </summary>
        public MessageCode[] ArrayMsgCode { get; init; } = new[] { MessageCodeOption.Success };
        /// <summary>
        /// 消息码
        /// </summary>
        public MessageCode MsgCode
        {
            get { return ArrayMsgCode[0]; }
            init { ArrayMsgCode = new[] { value }; }
        }
    }
}
