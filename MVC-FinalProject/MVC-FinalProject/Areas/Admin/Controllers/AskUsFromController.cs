
using Microsoft.AspNetCore.Mvc;

using MVC_FinalProject.Services.Interfaces;
using Newtonsoft.Json.Linq;

namespace MVC_FinalProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AskUsFromController : Controller
    {
        private readonly IAskUsFromService _askUsFromService;
        public AskUsFromController(IAskUsFromService askUsFromService)
        {
            _askUsFromService = askUsFromService;
        }

        public async Task<IActionResult> Index()
        {
            var messages = await _askUsFromService.GetAllAsync();
            return View(messages);
        }

        [HttpPost]
        public async Task<IActionResult> Approve(int id)
        {
            await _askUsFromService.ApproveMessageAsync(id);
            return RedirectToAction("Index");
        }
    }
}

