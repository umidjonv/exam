using System.Security.Claims;
using EX.Web.Consts;
using Microsoft.AspNetCore.Mvc;

namespace EX.Web.Controllers
{
    public abstract class BaseMvcController : Controller
    {

        protected string UserName => User.FindFirstValue(AuthConst.UserClaim);

    }
}
