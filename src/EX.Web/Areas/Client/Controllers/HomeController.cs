using Microsoft.AspNetCore.Mvc;

namespace EX.Web.Areas.Client.Controllers
{
    public class HomeController :  BaseMvcController
    { 
        public IActionResult Index()
        {
            return View();
        }
    }
}
