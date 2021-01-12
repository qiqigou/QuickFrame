using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace QuickFrame.Common
{
    /// <summary>
    /// 处理过程异常集合
    /// </summary>
    public class HandelArrayException : ApplicationException
    {
        private readonly List<HandelException> _handelExceptions = new List<HandelException>(5);
        public IReadOnlyList<HandelException> Exceptions => _handelExceptions;

        public HandelArrayException(IEnumerable<HandelException> exceptions)
        {
            _handelExceptions.AddRange(exceptions);
        }

        public HandelArrayException(MessageCode messageCode, IEnumerable ages)
        {
            foreach (var item in ages)
            {
                var exception = new HandelException(messageCode, item);
                _handelExceptions.Add(exception);
            }
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
