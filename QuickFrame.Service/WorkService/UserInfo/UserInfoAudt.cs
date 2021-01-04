using QuickFrame.Common;
using QuickFrame.Models;
using System;
using System.Threading.Tasks;

namespace QuickFrame.Services
{
    /// <summary>
    /// 用户审核
    /// </summary>
    [TransientInjection]
    public class UserInfoAudt : AudtHandleBase<userinfo_us, int>
    {
        protected override Task AudtAsync(AudtInput<int> input)
        {
            throw new NotImplementedException();
        }

        protected override Task UnAudtAsync(AudtInput<int> input)
        {
            throw new NotImplementedException();
        }

        protected override Task HandleAsync()
        {
            throw new NotImplementedException();
        }
    }
}
