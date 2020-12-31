using Mapster;
using QuickFrame.Model;

namespace QuickFrame.Service
{
    internal class SyscompanyMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<SysCompanyInput, syscompany_scy>();
            config.NewConfig<SysCompanyUpdInput, syscompany_scy>();
        }
    }
}
