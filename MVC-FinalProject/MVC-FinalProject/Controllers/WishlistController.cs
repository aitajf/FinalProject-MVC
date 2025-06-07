using Microsoft.AspNetCore.Mvc;

namespace MVC_FinalProject.Controllers
{
    public class WishlistController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
