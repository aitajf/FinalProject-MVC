using Microsoft.AspNetCore.Mvc;
using MVC_FinalProject.Services.Interfaces;
using MVC_FinalProject.ViewModels;
using NuGet.Configuration;

namespace MVC_FinalProject.Controllers
{
    public class MyAccountController : Controller
    {
        private readonly ISettingService _settingService;

        public MyAccountController(ISettingService settingService)
        {
            _settingService = settingService;
        }


        public async Task<IActionResult> Index()
        {
            var setting = await _settingService.GetAllAsync();

            MyAccountVM model = new MyAccountVM()
            {
                Setting = setting
            };

            return View(model);
        }
    }
}
