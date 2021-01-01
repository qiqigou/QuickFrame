using Mapster;
using QuickFrame.Model;
using System;

namespace QuickFrame.Service
{
    internal class UserInfoMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<UserInfoInput, userinfo_us>()
                .Map(dest => dest.birthday, src => src.Birthday ?? DateTime.Now);
            config.NewConfig<UserInfoUpdInput, userinfo_us>()
                .Map(dest => dest.birthday, src => src.Birthday ?? DateTime.Now);
        }
    }
}
