using System.Security.Claims;
using EX.Web.Consts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EX.Web.Areas.Client.Controllers
{
    [Authorize(AuthConst.ClientPolicy)]
    [Area("Client")]
    public abstract class BaseMvcController : Controller
    {

        protected string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier); 

    }
}
