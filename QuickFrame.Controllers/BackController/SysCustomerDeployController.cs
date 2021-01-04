using Microsoft.AspNetCore.Mvc;
using QuickFrame.Common;
using QuickFrame.IServices;
using QuickFrame.Models;

namespace QuickFrame.Controllers
{
    /// <summary>
    /// 客户部署
    /// </summary>
    [Area(ConstantOptions.ModulesConstant.Back)]
    [ApiGroup(ConstantOptions.ModulesConstant.Back)]
    public class SysCustomerDeployController : BaseController
    {
        private readonly ISysCustomerDeployService _sysCustomerDeployService;

        public SysCustomerDeployController(ISysCustomerDeployService sysCustomerDeployService)
        {
            _sysCustomerDeployService = sysCustomerDeployService;
        }
        /// <summary>
        /// 获取账套创建脚本
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ScriptOutput GetDbScriptAsync()
        {
            return _sysCustomerDeployService.GetDbScript();
        }
    }
}
