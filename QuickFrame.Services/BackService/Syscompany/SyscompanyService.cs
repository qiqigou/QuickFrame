using System;
using QuickFrame.IServices;
using QuickFrame.Models;
using QuickFrame.Repositorys;

namespace QuickFrame.Services
{
    /// <summary>
    /// 公司管理
    /// </summary>
    public class SysCompanyService : BillServiceBase<syscompany_scy, SysCompanyInput, SysCompanyUpdInput, v_syscompany, string>, ISysCompanyService
    {
        public SysCompanyService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        protected override IQueryProvider<v_syscompany, string> Query(IQueryFactory queryFactory)
            => queryFactory.Create<BackOption, v_syscompany, string>(px => px.scy_ccompanyid, px => new { px.scy_ccompanyid, px.scy_ccompanyname });
    }
}
