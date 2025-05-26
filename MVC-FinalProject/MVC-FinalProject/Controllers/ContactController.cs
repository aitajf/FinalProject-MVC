using Microsoft.AspNetCore.Mvc;

namespace MVC_FinalProject.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
