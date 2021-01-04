using QuickFrame.Common;
using System;
using System.ComponentModel.DataAnnotations;

namespace QuickFrame.Models
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public class userinfo_us : WithStampTable, IDbEntity<WorkOption>
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        [Key]
        public int uid { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        [StringColumn]
        public string name { get; set; } = string.Empty;
        /// <summary>
        /// 性别
        /// </summary>
        [StringColumn]
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
