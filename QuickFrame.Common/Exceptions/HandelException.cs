using System;

namespace QuickFrame.Common
{
    /// <summary>
    /// 处理过程异常
    /// </summary>
    public class HandelException : ApplicationException
    {
        /// <summary>
        /// 消息码
        /// </summary>
        public MessageCode MsgCode { get; }
        /// <summary>
        /// 处理过程异常
        /// </summary>
        /// <param name="messageCode"></param>
        /// <param name="msgargs"></param>
        public HandelException(MessageCode messageCode, params object[] msgargs)
        {
            var args = msgargs.Length > 0 ? msgargs : messageCode.MsgArgs;
            MsgCode = messageCode with { MsgArgs = args };
        }
        /// <summary>
        /// 消息内容
        /// </summary>
        public override string Message => MsgCode.Message;
    }
}
