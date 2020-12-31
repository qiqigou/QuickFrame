using System;
using QuickFrame.Model;

namespace QuickFrame.Service
{
    /// <summary>
    /// 商场表服务
    /// </summary>
    public class SysCompanybakService : BillServiceBase<syscompanybak_scb, SysCompanybakInput, SysCompanybakUpdInput, v_syscompanybak, (int, string)>, ISyscompanybakService
    {
        public SysCompanybakService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        protected override IQueryProvider<v_syscompanybak, (int, string)> Query(IQueryFactory queryFactory)
            => queryFactory.Create<BackOption, v_syscompanybak, (int, string)>(px => new(px.scb_corder, px.scy_ccompanyid));
    }
}
