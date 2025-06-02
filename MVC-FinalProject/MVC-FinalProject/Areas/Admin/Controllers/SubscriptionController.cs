using Microsoft.AspNetCore.Mvc;
using MVC_FinalProject.Services;
using MVC_FinalProject.Services.Interfaces;

namespace MVC_FinalProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SubscriptionController : Controller
    {
        private readonly ISubscriptionService _subscriptionService;
        public SubscriptionController(ISubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var res = await _subscriptionService.GetAllSubscriptionsAsync();
            return View(res);
        }
    }
}
