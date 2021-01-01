using Mapster;
using QuickFrame.Model;
using System;

namespace QuickFrame.Service
{
    public class SyscompanybakMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<SysCompanybakInput, syscompanybak_scb>()
                .Map(dest => dest.scb_ddate, src => DateTime.Now);
            config.NewConfig<SysCompanybakUpdInput, syscompanybak_scb>();
        }
    }
}
