using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC_FinalProject.Services;
using MVC_FinalProject.Services.Interfaces;

namespace MVC_FinalProject.Areas.Admin.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
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

        [HttpPost]
        public async Task<IActionResult> Unsubscribe(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                TempData["Error"] = "Email is required.";
                return RedirectToAction(nameof(Index));
            }

            var response = await _subscriptionService.UnsubscribeAsync(email);

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = $"User with email '{email}' has been unsubscribed.";
            }
            else
            {
                var errorMsg = await response.Content.ReadAsStringAsync();
            }

            return Ok();
        }
    }
}
