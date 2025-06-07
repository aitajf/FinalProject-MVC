using Microsoft.AspNetCore.Mvc;
using MVC_FinalProject.Services.Interfaces;

namespace MVC_FinalProject.Controllers
{
    public class FAQController : Controller
    {
        private readonly IAskUsFromService _askUsFromService;

        public FAQController(IAskUsFromService askUsFromService)
        {
            _askUsFromService = askUsFromService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var approvedMessages = await _askUsFromService.GetApprovedMessagesAsync();
            return View(approvedMessages);
        }
    }
}
