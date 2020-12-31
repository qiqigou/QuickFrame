using System;
using System.Threading.Tasks;
using QuickFrame.Common;
using QuickFrame.Model;

namespace QuickFrame.Service
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
