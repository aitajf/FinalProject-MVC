using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC_FinalProject.Services.Interfaces;

namespace MVC_FinalProject.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin")]
    [Area("Admin")]
    public class SettingController : Controller
    {
        private readonly ISettingService _settingService;
        public SettingController(ISettingService settingService)
        {
                _settingService = settingService;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _settingService.GetAllAsync());
        }
    }
}
