using Microsoft.AspNetCore.Mvc;

namespace EX.Web.Areas.Admin.Controllers
{
    public class HomeController : BaseMvcController
    {  
        public IActionResult Index()
        {
            return View();
        }
    }
}
