using Microsoft.AspNetCore.Mvc;
using MVC_FinalProject.Services.Interfaces;
using MVC_FinalProject.ViewModels;

namespace MVC_FinalProject.Controllers
{
    public class AboutController : Controller
    {
        private readonly IAboutBannerImgService _aboutBannerImgService;
        private readonly IBrandService _brandService;
        private readonly IAboutPromoService _aboutPromoService;
        public AboutController(IAboutBannerImgService aboutBannerImgService,
                               IBrandService brandService,
                               IAboutPromoService aboutPromoService)
        {
            _aboutBannerImgService = aboutBannerImgService;
            _brandService = brandService;
            _aboutPromoService = aboutPromoService;
        }
        public async Task<IActionResult> Index()
        {
            var bannerImg  = await _aboutBannerImgService.GetAllAsync();
            var brands = await _brandService.GetAllAsync();
            var promos = await _aboutPromoService.GetAllAsync();    

            AboutVM model = new()
            {
                AboutBannerImgs = bannerImg,
                Brands = brands,
                AboutPromos = promos
            };
            return View(model);
        }
    }
}
