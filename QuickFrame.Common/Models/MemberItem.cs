using System;

namespace QuickFrame.Common
{
    /// <summary>
    /// XML注释模型
    /// </summary>
    public class MemberItem : IMEntity
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// 描述
        /// </summary>
        public string Summary { get; set; } = string.Empty;
        /// <summary>
        /// 参数
        /// </summary>
        public NameValueItem[] Param { get; set; } = Array.Empty<NameValueItem>();
        /// <summary>
        /// 返回值
        /// </summary>
        public string Returns { get; set; } = string.Empty;
        /// <summary>
        /// 泛型参数
        /// </summary>
        public NameValueItem[] TypeParam { get; set; } = Array.Empty<NameValueItem>();
    }
}
