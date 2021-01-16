using System;
using Mapster;
using QuickFrame.Models;

namespace QuickFrame.Services
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
