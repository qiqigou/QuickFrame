using Microsoft.AspNetCore.Mvc.ApiExplorer;
using System;

namespace QuickFrame.Common
{
    /// <summary>
    /// API分组
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class ApiGroupAttribute : Attribute, IApiDescriptionGroupNameProvider
    {
        public string GroupName { get; set; } = string.Empty;

        public ApiGroupAttribute(string groupName)
        {
            GroupName = groupName;
        }
    }
}
