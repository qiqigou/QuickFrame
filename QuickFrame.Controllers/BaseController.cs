using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace QuickFrame.Controllers
{
    [Route("api/[area]/[controller]/[action]")]
    [Authorize]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    public class BaseController : ControllerBase
    {

    }
}
