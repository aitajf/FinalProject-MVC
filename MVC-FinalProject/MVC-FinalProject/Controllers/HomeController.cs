using Microsoft.AspNetCore.Mvc;
using MVC_FinalProject.Models.Slider;
using MVC_FinalProject.Services.Interfaces;
using MVC_FinalProject.ViewModels;

namespace MVC_FinalProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISliderService _sliderService;
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService  ;
        private readonly ILandingBannerService _landingBannerService;
        private readonly IInstagramService _instagramService;
        private readonly ISubscribeImgService _subscribeImgService;
        public HomeController(ISliderService sliderService,
                              ICategoryService categoryService,
                              IProductService productService,
                              ILandingBannerService landingBannerService,
                              IInstagramService instagramService,
                              ISubscribeImgService subscribeImgService)
        {
            _sliderService = sliderService;
            _categoryService = categoryService;
            _productService = productService;
            _instagramService = instagramService;
            _landingBannerService = landingBannerService;
            _subscribeImgService = subscribeImgService;               
        }

        public async Task<IActionResult> Index()
        {
            var sliders = await _sliderService.GetAllAsync();
            var categories = await _categoryService.GetAllAsync();
            var products = await _productService.GetAllAsync();
            var landingBanners = await _landingBannerService.GetAllAsync();
            var instagrams =  await _instagramService.GetAllAsync();
            var subscribeImgs =  await _subscribeImgService.GetAllAsync();

            HomeVM model = new HomeVM()
            {
                Sliders = sliders,
                Categories = categories,
                Products = products,
                LandingBanners = landingBanners,
                Instagrams = instagrams,
                SubscribeImgs = subscribeImgs
            };
            return View(model);
        }
    }
}
