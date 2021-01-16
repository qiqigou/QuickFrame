using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using QuickFrame.Common;
using QuickFrame.IServices;
using QuickFrame.Models;
using QuickFrame.Repositorys;

namespace QuickFrame.Services
{
    /// <summary>
    /// 字段过滤器服务
    /// </summary>
    public class FieldFilterService : BillMainChildServiceBase<fieldfilter_fg, fieldfilterc_fgc, v_fieldfilter, v_fieldfilterc, FieldFilterInput, FieldFilterUpdInput, FieldFiltercInput, FieldFiltercUpdInput, long, int>, IFieldFilterService
    {
        private readonly IFieldFilterProvider _filterProvider;

        public FieldFilterService(IServiceProvider serviceProvider, IFieldFilterProvider filterProvider) : base(serviceProvider)
        {
            _filterProvider = filterProvider;
        }

        protected override IQueryProvider<v_fieldfilter, long> QueryMain(IQueryFactory queryFactory)
            => queryFactory.Create<WorkOption, v_fieldfilter, long>(qkey => qkey.fg_id);
        protected override IQueryProvider<v_fieldfilterc, long> QueryChild(IQueryFactory queryFactory)
            => queryFactory.Create<WorkOption, v_fieldfilterc, long>(qkey => qkey.fg_id, order => new { order.fg_id, order.fgc_iorder });
        protected override Func<FieldFiltercUpdInput, int> ChildUpdInputIorder
            => x => x.FgcIorder;
        protected override Func<fieldfilterc_fgc, int> ChildIorder
            => x => x.fgc_iorder;
        protected override Expression<Func<fieldfilter_fg, IEnumerable<fieldfilterc_fgc>?>> ChildTable
            => x => x.fieldfilterc_fgc;

        public override async Task<long> CreateMainChildAsync(MainChildInput<FieldFilterInput, FieldFiltercInput> input)
        {
            var res = await base.CreateMainChildAsync(input);
            _filterProvider.RemoveNewType(input.Main.FgSource, input.Main.FgDest);
            return res;
        }

        public override async Task<int> UpdateMainChildAsync(long keyValue, byte[] timestamp, MainChildInput<FieldFilterUpdInput, FieldFiltercUpdInput> input)
        {
            var res = await base.UpdateMainChildAsync(keyValue, timestamp, input);
            _filterProvider.RemoveNewType(input.Main.FgSource ?? string.Empty, input.Main.FgDest);
            return res;
        }

        public override async Task<int> DeleteMainChildAsync(KeyStamp<long>[] arrayKeyStamp)
        {
            _mainRepository.DeletedByKey += RemoveNewTypes;
            var count = await base.DeleteMainChildAsync(arrayKeyStamp);
            _mainRepository.DeletedByKey -= RemoveNewTypes;
            return count;

            void RemoveNewTypes(object? sender, IEnumerable<fieldfilter_fg> e)
            {
                e.ForEach(item =>
                {
                    _filterProvider.RemoveNewType(item.fg_source, item.fg_dest);
                });
            }
        }
    }
}
