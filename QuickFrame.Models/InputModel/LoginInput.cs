using QuickFrame.Common;

namespace QuickFrame.Models
{
    public class LoginInput : IDataInput
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [StringColumn(8)]
        public string UserName { get; set; } = string.Empty;
        /// <summary>
        /// 密码
        /// </summary>
        [StringLengthInput(16, MinimumLength = 6)]
        public string PassWord { get; set; } = string.Empty;
        /// <summary>
        /// 账套
        /// </summary>
        /// <value></value>
        [StringLengthInput(10, MinimumLength = 4)]
        public string DbName { get; set; } = string.Empty;
    }
}
