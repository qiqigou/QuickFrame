namespace QuickFrame.Common
{
    /// <summary>
    /// 预设返回值记录
    /// </summary>
    public class HandleResultOption
    {
        /// <summary>
        /// 成功
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static HandleResult<TValue> Success<TValue>(TValue value) => new HandleResult<TValue> { Value = value };
        /// <summary>
        /// 失败
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        public static HandleResult<TValue> Bad<TValue>(MessageCode msgcode) => new HandleResult<TValue> { Success = false, MsgCode = msgcode };
    }
}
