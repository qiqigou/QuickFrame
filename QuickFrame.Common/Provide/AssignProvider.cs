using ObjAssignMap;
using System;

namespace QuickFrame.Common
{
    /// <summary>
    /// 赋值器提供者
    /// </summary>
    [SingletonInjection]
    public class AssignProvider : IAssignProvider
    {
        private readonly IUser _user;

        public AssignProvider(IUser user)
        {
            _user = user;
        }
        /// <summary>
        /// 构建赋值器
        /// </summary>
        /// <returns></returns>
        public IObjAssign CreateAssign()
        {
            var config = new AssignConfig();
            //建立人
            config.ForField<string>(AssignOption.Create)
                .SetRule(field => field.EndsWith("beginman"))
                .SetVal(p => _user.Id);
            //建立日期
            config.ForField<DateTime?>(AssignOption.Create)
                .SetRule(field => field.EndsWith("begindt"))
                .SetVal(p => DateTime.Now);
            //修改人
            config.ForField<string>(AssignOption.Update)
                .SetRule(field => field.EndsWith("editman"))
                .SetVal(p => _user.Id);
            //修改日期
            config.ForField<DateTime?>(AssignOption.Update)
                .SetRule(field => field.EndsWith("editdt") || field.Equals("vv_deitdt"))
                .SetVal(p => DateTime.Now);
            //审核\弃审 人
            config.ForField<string>(AssignOption.Auth, AssignOption.UnAuth)
                .SetRule(field => field.EndsWith("_cmtman"))
                .SetVal(p => _user.Id);
            //审核\弃审 日期
            config.ForField<DateTime?>(AssignOption.Auth, AssignOption.UnAuth)
                .SetRule(field => field.EndsWith("_cmtdt") || field.EndsWith("_dmtdt") || field.EndsWith("_dcmtdt"))
                .SetVal(p => DateTime.Now);
            //审核标识
            config.ForField<string>(AssignOption.Auth)
                .SetRule(field => field.EndsWith("_cauditflag") || field.EndsWith("_cmtflag"))
                .SetVal(p => ConstantOptions.DbFlag.DbTrue);
            //弃审标识
            config.ForField<string>(AssignOption.UnAuth)
                .SetRule(field => field.EndsWith("_cauditflag") || field.EndsWith("_cmtflag"))
                .SetVal(p => ConstantOptions.DbFlag.DbFalse);
            return config.Build();
        }
    }
}
