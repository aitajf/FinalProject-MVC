using Microsoft.AspNetCore.Mvc;
using MVC_FinalProject.Services.Interfaces;
using MVC_FinalProject.ViewModels;

namespace MVC_FinalProject.Controllers
{
    public class AboutController : Controller
    {
        private readonly IAboutBannerImgService _aboutBannerImgService;
        private readonly IBrandService _brandService;
        public AboutController(IAboutBannerImgService aboutBannerImgService,
                               IBrandService brandService)
        {
            _aboutBannerImgService = aboutBannerImgService;
            _brandService = brandService;
        }
        public async Task<IActionResult> Index()
        {
            var bannerImg  = await _aboutBannerImgService.GetAllAsync();
            var brands = await _brandService.GetAllAsync();

            AboutVM model = new()
            {
                AboutBannerImgs = bannerImg,
                Brands = brands
            };
            return View(model);
        }
    }
}
