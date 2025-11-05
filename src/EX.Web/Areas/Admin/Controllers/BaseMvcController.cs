using System.Security.Claims;
using EX.Web.Consts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace EX.Web.Areas.Admin.Controllers
{
    [Authorize(AuthConst.AdminPolicy)]
    [Area("Admin")]
    public abstract class BaseMvcController : Controller
    {
        protected string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}
