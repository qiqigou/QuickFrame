using Mapster;
using QuickFrame.Models;

namespace QuickFrame.Services
{
    public class SyscompanyshopMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<SysCompanyshopInput, syscompanyshop_scs>();
            config.NewConfig<SysCompanyshopUpdInput, syscompanyshop_scs>();
        }
    }
}
