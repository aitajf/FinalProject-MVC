using Microsoft.AspNetCore.Mvc;

namespace MVC_FinalProject.Controllers
{
    public class ShopController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
