using System;
using System.Threading.Tasks;
using QuickFrame.Common;
using QuickFrame.IServices;
using QuickFrame.Models;
using QuickFrame.Repositorys;

namespace QuickFrame.Services
{
    public class UserInfoService : BillServiceBase<userinfo_us, UserInfoInput, UserInfoUpdInput, v_userinfo, int>, IUserInfoService
    {
        private readonly IAudtHandle<userinfo_us, int> _audtHandle;
        private readonly IApproveHandle<userinfo_us, int> _approveHandle;

        public UserInfoService(IServiceProvider serviceProvider, IAudtHandle<userinfo_us, int> audtHandle, IApproveHandle<userinfo_us, int> approveHandle) : base(serviceProvider)
        {
            _audtHandle = audtHandle;
            _approveHandle = approveHandle;
        }

        protected override IQueryProvider<v_userinfo, int> Query(IQueryFactory queryFactory)
            => queryFactory.Create<WorkOption, v_userinfo, int>(qkey => qkey.uid, order => new { order.uid, order.name });
        /// <summary>
        /// 创建用户(测试AOP)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AutoProxy(nameof(TestProxyHandle), nameof(Test1ProxyHandle))]
        public override Task<int> CreateAsync(UserInfoInput input) => base.CreateAsync(input);

        public Task AudtRangeAsync(int[] keys) => _audtHandle.AudtRangeAsync(keys);

        public Task UnAudtRangeAsync(int[] keys) => _audtHandle.UnAudtRangeAsync(keys);

        public Task ApproveRangeAsync(int[] keys) => _approveHandle.ApproveRangeAsync(keys);

        public Task UnApproveRangeAsync(int[] keys) => _approveHandle.UnApproveRangeAsync(keys);
    }
}
