using QuickFrame.Common;
using System;

namespace QuickFrame.Models
{
    /// <summary>
    /// 创建用户
    /// </summary>
    public class UserInfoInput : IDataInput
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [StringColumn]
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// 性别
        /// </summary>
        [StringColumn]
        public string Sex { get; set; } = string.Empty;
        /// <summary>
        /// 年龄
        /// </summary>
        public int Age { get; set; }
        /// <summary>
        /// 生日
        /// </summary>
        public DateTime? Birthday { get; set; }
    }
    /// <summary>
    /// 修改用户
    /// </summary>
    public class UserInfoUpdInput : IDataInput
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [StringColumn]
        public string? Name { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        [StringColumn]
        public string? Sex { get; set; }
        /// <summary>
        /// 年龄
        /// </summary>
        public int? Age { get; set; }
        /// <summary>
        /// 生日
        /// </summary>
        public DateTime? Birthday { get; set; }
    }
}
