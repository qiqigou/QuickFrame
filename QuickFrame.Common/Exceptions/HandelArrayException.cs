using System;
using System.Collections.Generic;
using System.Linq;

namespace QuickFrame.Common
{
    /// <summary>
    /// 处理过程异常集合
    /// </summary>
    public class HandelArrayException : ApplicationException
    {
        private readonly List<HandelException> _handelExceptions;
        public IReadOnlyList<HandelException> Exceptions => _handelExceptions;

        public HandelArrayException(HandelException exception)
        {
            _handelExceptions = new List<HandelException>
            {
                exception
            };
        }
        /// <summary>
        /// 添加异常
        /// </summary>
        /// <param name="exceptions"></param>
        /// <returns></returns>
        public HandelArrayException Append(HandelException exceptions)
        {
            _handelExceptions.Add(exceptions);
            return this;
        }
        /// <summary>
        /// 消息集合
        /// </summary>
        public string[] ArrayMessage => _handelExceptions.Select(x => x.Message).ToArray();
        /// <summary>
        /// 消息内容
        /// </summary>
        public override string Message => string.Join(Environment.NewLine, ArrayMessage);
    }
}
