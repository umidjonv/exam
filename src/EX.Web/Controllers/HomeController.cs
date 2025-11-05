using System;
using System.Linq;
using System.Threading.Tasks;
using EX.Data.Core;
using EX.Web.Consts;
using Microsoft.AspNetCore.Mvc;
using EX.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Authorization;
using X.PagedList;

namespace EX.Web.Controllers
{
    public class HomeController : BaseMvcController
    {
        private readonly IAppDbContext _db;

        public HomeController(IAppDbContext db)
        {
            _db = db;
        }

        [Authorize]
        public IActionResult Profile()
        {
            if (UserName == AuthConst.AdminRole)
            {
                return RedirectToAction("Index", "Result", new { area = "Admin" });
            }
            else
            {
                return RedirectToAction("Index", "Exam", new { area = "Client" });
            }
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Docs = await _db.Documents.OrderByDescending(a => a.ModifiedDate).ToPagedListAsync(1, 5);

            return View();
        }

        [HttpGet("~/signin")]
        public IActionResult SignIn(string redirectUrl = "/home/profile")
        {
            return Challenge(new AuthenticationProperties
            {
                RedirectUri = redirectUrl
            }, OpenIdConnectDefaults.AuthenticationScheme);
        }

        [HttpGet("~/signout")]
        public IActionResult SignOut(string redirectUrl = "/")
        {
            return SignOut(new AuthenticationProperties
            {
                RedirectUri = redirectUrl
            }, OpenIdConnectDefaults.AuthenticationScheme,
                CookieAuthenticationDefaults.AuthenticationScheme);
        }

        public IActionResult ChangeLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddDays(7)
                });

            return LocalRedirect(returnUrl);
        }

        [Route("~/error ")]
        public IActionResult Error( )
        { 
            var feature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            if (feature == null)
                return View();

            return View(new ErrorViewModel
            {
                Path = feature.Path,
                Exception = feature.Error,
            });
        }
    }
}
