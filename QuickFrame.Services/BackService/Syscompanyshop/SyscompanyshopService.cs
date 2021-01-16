using System;
using Mapster;
using QuickFrame.IServices;
using QuickFrame.Models;
using QuickFrame.Repositorys;

namespace QuickFrame.Services
{
    public class SysCompanyshopService : BillServiceBase<syscompanyshop_scs, SysCompanyshopInput, SysCompanyshopUpdInput, v_syscompanyshop, (string, string)>, ISysCompanyshopService
    {
        public SysCompanyshopService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        protected override IQueryProvider<v_syscompanyshop, (string, string)> Query(IQueryFactory queryFactory)
            => queryFactory.Create<BackOption, v_syscompanyshop, (string, string)>(px => new(px.scs_cshopid, px.scy_ccompanyid));
    }
    /// <summary>
    /// 配置映射
    /// </summary>
    public class SyscompanyshopMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<SysCompanyshopInput, syscompanyshop_scs>();
            config.NewConfig<SysCompanyshopUpdInput, syscompanyshop_scs>();
        }
    }
}
