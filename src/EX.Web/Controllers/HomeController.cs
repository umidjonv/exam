using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EX.Data.Core;
using EX.Web.Consts;
using Microsoft.AspNetCore.Mvc;
using EX.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Diagnostics;
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
        public IActionResult SignIn(string returnUrl = "/home/profile")
        {
            if (User.Identity.IsAuthenticated)
            {
                return LocalRedirect(returnUrl);
            }

            return View("Login", new Models.LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost("~/login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Models.LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Login", model);
            }

            // Simple authentication - validates only that username is provided
            // In production, this should validate against a database or identity provider
            if (string.IsNullOrEmpty(model.Username))
            {
                ModelState.AddModelError("", "Username is required");
                return View("Login", model);
            }

            // Determine role based on username
            var role = model.Username.ToLower() == AuthConst.AdminRole ? AuthConst.AdminRole : "user";

            // Create claims for the user
            var claims = new List<System.Security.Claims.Claim>
            {
                new System.Security.Claims.Claim(AuthConst.UserClaim, model.Username),
                new System.Security.Claims.Claim(ClaimTypes.Name, model.Username),
                new System.Security.Claims.Claim(ClaimTypes.Role, role),
                new System.Security.Claims.Claim(ClaimTypes.NameIdentifier, model.Username)
            };

            var claimsIdentity = new System.Security.Claims.ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(24)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new System.Security.Claims.ClaimsPrincipal(claimsIdentity),
                authProperties);

            return LocalRedirect(model.ReturnUrl ?? "/home/profile");
        }

        [HttpGet("~/signout")]
        public async Task<IActionResult> SignOut(string redirectUrl = "/")
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return LocalRedirect(redirectUrl);
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
