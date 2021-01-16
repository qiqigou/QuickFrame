using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QuickFrame.Common;
using QuickFrame.IServices;
using QuickFrame.Models;

namespace QuickFrame.Controllers
{
    /// <summary>
    /// 字段过滤
    /// </summary>
    [Area(ConstantOptions.ModulesConstant.Work)]
    [ApiGroup(ConstantOptions.ModulesConstant.Work)]
    public class FieldFilterController : BaseController
    {
        private readonly IFieldFilterService _fieldFilterService;

        public FieldFilterController(IFieldFilterService fieldFilterService)
        {
            _fieldFilterService = fieldFilterService;
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:long}")]
        public Task<MainChildOutput<v_fieldfilter, v_fieldfilterc>?> SingleMainChildAsync([FromRoute] long id)
        {
            return _fieldFilterService.SingleMainChildAsync(id);
        }
        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<KeyOutput<long>> CreateMainChildAsync([FromBody] MainChildInput<FieldFilterInput, FieldFiltercInput> input)
        {
            var keyvalue = await _fieldFilterService.CreateMainChildAsync(input);
            return new KeyOutput<long>(keyvalue);
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="id"></param>
        /// <param name="timestamp"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("{id:long}/{timestamp}")]
        public async Task<MsgOutput> UpdateMainChildAsync([FromRoute] long id, [FromRoute] byte[] timestamp, [FromBody] MainChildInput<FieldFilterUpdInput, FieldFiltercUpdInput> input)
        {
            await _fieldFilterService.UpdateMainChildAsync(id, timestamp, input);
            return MsgOutputOption.OkMsg;
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        [HttpDelete("{id:long}/{timestamp}")]
        public async Task<MsgOutput> DeleteMainChildAsync([FromRoute] long id, [FromRoute] byte[] timestamp)
        {
            var key = new KeyStamp<long>(id, timestamp);
            await _fieldFilterService.DeleteMainChildAsync(new[] { key });
            return MsgOutputOption.OkMsg;
        }
        /// <summary>
        /// 主视图条件查询
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public Task<PageOutput<v_fieldfilter>> ObjQueryMainAsync([FromBody] ObjFilterInput input)
        {
            return _fieldFilterService.ObjQueryMainAsync(input);
        }
        /// <summary>
        /// 主视图条件查询
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public Task<PageOutput<v_fieldfilter>> SQLQueryMainAsync([FromBody] SQLFilterInput input)
        {
            return _fieldFilterService.SQLQueryMainAsync(input);
        }
    }
}
