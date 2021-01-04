using System;

namespace QuickFrame.Models
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public class v_userinfo : WithStampView, IDbEntity<WorkOption>
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public int uid { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string name { get; set; } = string.Empty;
        /// <summary>
        /// 性别
        /// </summary>
        public string sex { get; set; } = string.Empty;
        /// <summary>
        /// 年龄
        /// </summary>
        public int age { get; set; }
        /// <summary>
        /// 生日
        /// </summary>
        public DateTime? birthday { get; set; }
    }
}
