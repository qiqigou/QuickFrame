using Microsoft.EntityFrameworkCore;
using QuickFrame.Models;
using System.Linq;

namespace QuickFrame.Repository
{
    /// <summary>
    /// 字段过滤仓储
    /// </summary>
    internal class FieldFilterRepository : RepositoryBase<fieldfilter_fg, long>, IFieldFilterRepository
    {
        public FieldFilterRepository(IUnitOfWork<WorkOption> unitOfWork) : base(unitOfWork)
        {
        }

        public string[]? GetFields(string sourceName, string destName)
        {
            var field = Select.Include(x => x.fieldfilterc_fgc).SingleOrDefault(x => x.fg_source == sourceName && x.fg_dest == destName);
            if (field?.fieldfilterc_fgc?.Any() ?? false)
            {
                return field.fieldfilterc_fgc.Select(x => x.fgc_field).ToArray();
            }
            return default;
        }
    }
    /// <summary>
    /// 字段过滤子仓储
    /// </summary>
    internal class FieldDtlFilterRepository : RepositoryBase<fieldfilterc_fgc, (long, int)>, IFieldFilterDtlRepository
    {
        public FieldDtlFilterRepository(IUnitOfWork<WorkOption> unitOfWork) : base(unitOfWork)
        {

        }
    }
}
