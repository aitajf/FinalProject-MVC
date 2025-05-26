using Microsoft.AspNetCore.Mvc;

namespace MVC_FinalProject.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
