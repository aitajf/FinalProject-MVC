using Microsoft.AspNetCore.Mvc;
using MVC_FinalProject.Services.Interfaces;
using MVC_FinalProject.ViewModels;

namespace MVC_FinalProject.Controllers
{
    public class WishlistController : Controller
    {
        private readonly ISettingService _settingService;

        public WishlistController(ISettingService settingService)
        {
            _settingService = settingService;
        }
        public async Task<IActionResult> Index()
        {
            var setting = await _settingService.GetAllAsync();

            WishlistVM model = new WishlistVM()
            {
                Setting = setting
            };
            return View(model);
        }
    }
}
