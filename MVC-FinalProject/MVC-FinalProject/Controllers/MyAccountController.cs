using Microsoft.AspNetCore.Mvc;

namespace MVC_FinalProject.Controllers
{
    public class MyAccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
