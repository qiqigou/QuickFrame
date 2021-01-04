using QuickFrame.IServices;
using QuickFrame.Models;
using QuickFrame.Repository;
using System;

namespace QuickFrame.Services
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
