using System;
using QuickFrame.Model;

namespace QuickFrame.Service
{
    public class SysCompanyshopService : BillServiceBase<syscompanyshop_scs, SysCompanyshopInput, SysCompanyshopUpdInput, v_syscompanyshop, (string, string)>, ISysCompanyshopService
    {
        public SysCompanyshopService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        protected override IQueryProvider<v_syscompanyshop, (string, string)> Query(IQueryFactory queryFactory)
            => queryFactory.Create<BackOption, v_syscompanyshop, (string, string)>(px => new(px.scs_cshopid, px.scy_ccompanyid));
    }
}
