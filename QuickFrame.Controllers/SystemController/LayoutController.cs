using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuickFrame.Common;
using QuickFrame.IServices;
using QuickFrame.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuickFrame.Controllers
{
    /// <summary>
    /// 布局端点
    /// </summary>
    [Area(ConstantOptions.ModulesConstant.System)]
    [ApiGroup(ConstantOptions.ModulesConstant.System)]
    public class LayoutController : BaseController
    {
        private readonly ILayoutService _layoutService;
        public LayoutController(ILayoutService layoutService)
        {
            _layoutService = layoutService;
        }
        /// <summary>
        /// 获取视图列
        /// </summary>
        /// <param name="viewName">视图名称</param>
        [HttpGet("{viewName}")]
        [AllowAnonymous]
        public Task<IEnumerable<ColumnItemOutput>> QueryViewColumnsAsync([FromRoute] string viewName)
        {
            return _layoutService.QueryViewColumnsAsync(viewName);
        }
        /// <summary>
        /// 获取所有视图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public Task<IEnumerable<ViewItemOutput>> QueryViewsAsync()
        {
            return _layoutService.QueryViewsAsync();
        }
    }
}
