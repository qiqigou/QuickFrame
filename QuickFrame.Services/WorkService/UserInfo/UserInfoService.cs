using QuickFrame.Common;
using QuickFrame.Models;
using QuickFrame.Repositorys;
using System;
using System.Threading.Tasks;

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

        public Task AudtRangeAsync(AudtInput<int>[] inputs) => _audtHandle.AudtRangeAsync(inputs);

        public Task UnAudtRangeAsync(AudtInput<int>[] inputs) => _audtHandle.UnAudtRangeAsync(inputs);

        public Task ApproveRangeAsync(AudtInput<int>[] inputs) => _approveHandle.ApproveRangeAsync(inputs);

        public Task UnApproveRangeAsync(AudtInput<int>[] inputs) => _approveHandle.UnApproveRangeAsync(inputs);
    }
}
