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
        /// <param name="msgobj"></param>
        public HandelException(MessageCode messageCode, params object[] msgobj)
        {
            var args = msgobj.Length > 0 ? msgobj : messageCode.MsgArgs;
            MsgCode = new MessageCode(messageCode.Code, messageCode.Title, messageCode.MsgTemplate, args);
        }
        /// <summary>
        /// 消息内容
        /// </summary>
        public override string Message => MsgCode.Message;
    }
}
