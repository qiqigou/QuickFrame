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
        public static HandleResult<TValue> Success<TValue>(TValue? value) => new HandleResult<TValue> { Value = value };
        /// <summary>
        /// 失败
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="msgcode"></param>
        /// <param name="msgargs"></param>
        /// <returns></returns>
        public static HandleResult<TValue> Bad<TValue>(MessageCode msgcode, params object[] msgargs)
        {
            var args = msgargs.Length > 0 ? msgargs : msgcode.MsgArgs;
            return new HandleResult<TValue> { Success = false, MsgCode = msgcode with { MsgArgs = args } };
        }
    }
}
