using Microsoft.AspNetCore.Mvc;
using MVC_FinalProject.Models.Account;
using MVC_FinalProject.Services.Interfaces;
using MVC_FinalProject.ViewModels;
using NuGet.Configuration;

namespace MVC_FinalProject.Controllers
{
    public class MyAccountController : Controller
    {
        private readonly ISettingService _settingService;
        private readonly IAccountService _accountService;

        public MyAccountController(ISettingService settingService, IAccountService accountService)
        {
            _settingService = settingService;
            _accountService = accountService;
        }


        public async Task<IActionResult> Index()
        {
            var setting = await _settingService.GetAllAsync();

            MyAccountVM model = new MyAccountVM()
            {
                Setting = setting,
                UpdateEmail = new UpdateEmail(),
                UpdateUsername = new UpdateUsername()
            };

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeEmail(UpdateEmail model)
        {
            var token = HttpContext.Session.GetString("AuthToken");
            if (string.IsNullOrEmpty(token)) return RedirectToAction("Login", "Account");

            if (string.IsNullOrWhiteSpace(model.NewEmail))
            {
                TempData["EmailMessage"] = "Email is required.";
                return RedirectToAction(nameof(Index));
            }

            var result = await _accountService.UpdateEmailAsync(model, token);
            TempData["EmailMessage"] = result;

            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeUsername(UpdateUsername model)
        {
            var token = HttpContext.Session.GetString("AuthToken");
            if (string.IsNullOrEmpty(token)) return RedirectToAction("Login", "Account");

            if (string.IsNullOrWhiteSpace(model.NewUsername))
            {
                TempData["UsernameMessage"] = "Username is required.";
                return RedirectToAction(nameof(Index));
            }

            var result = await _accountService.UpdateUsernameAsync(model, token);
            TempData["UsernameMessage"] = result;

            return RedirectToAction("Login", "Account");
        }
    }
}
