using Mapster;
using QuickFrame.Model;

namespace QuickFrame.Service
{
    internal class FieldFilterMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<FieldFilterInput, fieldfilter_fg>()
            .NameMatchingStrategy(NameMatchingStrategy.Flexible);

            config.NewConfig<FieldFilterUpdInput, fieldfilter_fg>()
            .NameMatchingStrategy(NameMatchingStrategy.Flexible);

            config.NewConfig<FieldFiltercInput, fieldfilterc_fgc>()
            .NameMatchingStrategy(NameMatchingStrategy.Flexible);
        }
    }
}
