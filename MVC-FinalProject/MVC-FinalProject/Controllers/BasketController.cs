using Microsoft.AspNetCore.Mvc;

namespace MVC_FinalProject.Controllers
{
    public class BasketController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
