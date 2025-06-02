using Microsoft.AspNetCore.Mvc;
using MVC_FinalProject.Models.Subscription;
using MVC_FinalProject.Services.Interfaces;
using MVC_FinalProject.ViewModels;

namespace MVC_FinalProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISliderService _sliderService;
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        private readonly ILandingBannerService _landingBannerService;
        private readonly IInstagramService _instagramService;
        private readonly ISubscribeImgService _subscribeImgService;
        private readonly ISubscriptionService _subscriptionService;

        public HomeController(ISliderService sliderService,
                              ICategoryService categoryService,
                              IProductService productService,
                              ILandingBannerService landingBannerService,
                              IInstagramService instagramService,
                              ISubscribeImgService subscribeImgService,
                              ISubscriptionService subscriptionService)
        {
            _sliderService = sliderService;
            _categoryService = categoryService;
            _productService = productService;
            _instagramService = instagramService;
            _landingBannerService = landingBannerService;
            _subscribeImgService = subscribeImgService;
            _subscriptionService = subscriptionService;
        }

        public async Task<IActionResult> Index()
        {
            var sliders = await _sliderService.GetAllAsync();
            var categories = await _categoryService.GetAllAsync();
            var products = await _productService.GetAllAsync();
            var landingBanners = await _landingBannerService.GetAllAsync();
            var instagrams = await _instagramService.GetAllAsync();
            var subscribeImgs = await _subscribeImgService.GetAllAsync();

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


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Subscribe(HomeVM model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        TempData["SubscribeError"] = "Email is required.";
        //        return RedirectToAction("Index");
        //    }
        //    var response = await _subscriptionService.SubscribeAsync(model.Subscribe);

        //    if (response.IsSuccessStatusCode)
        //    {
        //        TempData["SubscribeSuccess"] = "You have successfully subscribed !";
        //    }
        //    else
        //    {
        //        var error = await response.Content.ReadAsStringAsync();
        //        TempData["SubscribeError"] = $"Xəta baş verdi";
        //    }
        //    return RedirectToAction("Index");
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Subscribe(HomeVM model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        TempData["SubscribeError"] = "Email is required.";
        //        return RedirectToAction("Index");
        //    }

        //    var response = await _subscriptionService.SubscribeAsync(model.Subscribe);

        //    if (response.IsSuccessStatusCode)
        //    {
        //        TempData["SubscribeSuccess"] = "You have successfully subscribed!";
        //    }
        //    else
        //    {
        //        var errorMessage = await response.Content.ReadAsStringAsync();
        //        if (errorMessage.Contains("First be register"))
        //        {
        //            TempData["SubscribeError"] = "Please register before subscribing.";
        //        }
        //        else if (errorMessage.Contains("Subscribe with this mail already exists!"))
        //        {
        //            TempData["SubscribeError"] = "You have already subscribed with this email.";
        //        }
        //        else
        //        {
        //            TempData["SubscribeError"] = "An error occurred. Please try again later.";
        //        }
        //    }

        //    return RedirectToAction("Index");
        //}

        //BU DUZGUN VERSIYADIR  LAKIN REFRESHLE, SILMEYE QORXURAM ;(


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubscribeAjax([FromForm] string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return Json(new { success = false, message = "Email is required." });

            try
            {
                var dto = new SubscriptionCreate { Email = email };
                var response = await _subscriptionService.SubscribeAsync(dto);

                if (response.IsSuccessStatusCode)
                {
                    return Json(new { success = true, message = "You have successfully subscribed!" });
                }

                var errorMessage = await response.Content.ReadAsStringAsync();

                if (errorMessage.Contains("First be register"))
                    return Json(new { success = false, message = "Please register before subscribing." });

                if (errorMessage.Contains("already exists"))
                    return Json(new { success = false, message = "You have already subscribed with this email." });

                return Json(new { success = false, message = "An error occurred. Please try again later." });
            }
            catch
            {
                return Json(new { success = false, message = "An unexpected error occurred." });
            }
        }
    }
}