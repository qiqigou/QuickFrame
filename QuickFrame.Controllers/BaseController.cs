using System.Net.Mime;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace QuickFrame.Controllers
{
    [Route("api/[area]/[controller]/[action]")]
    [Authorize]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    public abstract class BaseController : ControllerBase { }
}
