using System;

namespace QuickFrame.Common
{
    /// <summary>
    /// 常量选项
    /// </summary>
    public static class ConstantOptions
    {
        /// <summary>
        /// 预设角色常量
        /// </summary>
        public static class RoleConstant
        {
            /// <summary>
            /// 系统角色(表示系统本身)
            /// </summary>
            public const string System = "system";
            /// <summary>
            /// 管理员角色
            /// </summary>
            public const string Admin = "admin";
            /// <summary>
            /// 所有预设角色
            /// </summary>
            public readonly static string[] AllRoles = { System, Admin };
        }
        /// <summary>
        /// 数据库Boolean值常量
        /// </summary>
        public static class DbFlagConstant
        {
            /// <summary>
            /// 1
            /// </summary>
            public const string DbTrue = "1";
            /// <summary>
            /// 0
            /// </summary>
            public const string DbFalse = "0";
        }
        /// <summary>
        /// 模块名常量
        /// </summary>
        public static class ModulesConstant
        {
            /// <summary>
            /// 业务模块
            /// </summary>
            public const string Work = "work";
            /// <summary>
            /// 后台模块
            /// </summary>
            public const string Back = "back";
            /// <summary>
            /// 系统模块
            /// </summary>
            public const string System = "system";
            /// <summary>
            /// 所有模块
            /// </summary>
            public readonly static string[] Modules = { System, Work, Back };
        }
        /// <summary>
        /// 比较符常量
        /// </summary>
        public static class CompareConstant
        {
            /// <summary>
            /// 等于
            /// </summary>
            public const string Equal = "equal";
            /// <summary>
            /// 不等于
            /// </summary>
            public const string NotEqual = "notequal";
            /// <summary>
            /// 小于
            /// </summary>
            public const string Less = "less";
            /// <summary>
            /// 大于
            /// </summary>
            public const string Greater = "greater";
            /// <summary>
            /// 小于等于
            /// </summary>
            public const string LessEq = "lesseq";
            /// <summary>
            /// 大于等于
            /// </summary>
            public const string GreaterEq = "greatereq";
            /// <summary>
            /// 包含
            /// </summary>
            public const string Contains = "contains";
            /// <summary>
            /// 所有比较符
            /// </summary>
            public readonly static string[] Compares = { Equal, NotEqual, Less, Greater, LessEq, GreaterEq, Contains };
        }
        /// <summary>
        /// 逻辑符常量
        /// </summary>
        public static class LogicConstant
        {
            /// <summary>
            /// 并且
            /// </summary>
            public const string And = "and";
            /// <summary>
            /// 或者
            /// </summary>
            public const string Or = "or";
        }
        /// <summary>
        /// 常用数据类型名常量
        /// </summary>
        public static class BaseDataTypeConstant
        {
            public const string Int16 = nameof(System.Int16);
            public const string Int32 = nameof(System.Int32);
            public const string Int64 = nameof(System.Int64);
            public const string UInt16 = nameof(System.UInt16);
            public const string UInt32 = nameof(System.UInt32);
            public const string UInt64 = nameof(System.UInt64);
            public const string Byte = nameof(System.Byte);
            public const string SByte = nameof(System.SByte);
            public const string Double = nameof(System.Double);
            public const string Float = nameof(Single);
            public const string Decimal = nameof(System.Decimal);
            public const string Bool = nameof(Boolean);
            public const string Char = nameof(System.Char);
            public const string DateTime = nameof(System.DateTime);
            public const string String = nameof(System.String);
        }
    }
}
