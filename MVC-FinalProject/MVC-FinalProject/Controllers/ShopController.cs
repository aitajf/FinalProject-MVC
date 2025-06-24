using Microsoft.AspNetCore.Mvc;
using MVC_FinalProject.Models.Product;
using MVC_FinalProject.Services.Interfaces;
using MVC_FinalProject.ViewModels;

namespace MVC_FinalProject.Controllers
{
    public class ShopController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly ITagService _tagService;
        private readonly IBrandService _brandService;
        private readonly ISettingService _settingService;
        public ShopController(IProductService productService, 
                              ICategoryService categoryService,
                              IBrandService brandService,
                              ITagService tagService,
                              ISettingService settingService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _brandService = brandService;
            _tagService = tagService;
            _settingService = settingService;
        }

        public async Task<IActionResult> Index(
          string categoryName,
          string colorName,
          string tagName,
          string brandName,
          string sortType,
          decimal? minPrice,
          decimal? maxPrice
      )
        {
            var categories = await _categoryService.GetAllAsync();
            var brands = await _brandService.GetAllAsync();
            var tags = await _tagService.GetAllAsync();
            var productCount = await _productService.GetProductsCountAsync();
            var categoryProductCounts = await _categoryService.GetCategoryProductCountsAsync();
            var brandProductCounts = await _brandService.GetBrandProductCountsAsync();
            var setting = await _settingService.GetAllAsync();

            var products = string.IsNullOrEmpty(categoryName) &&
                           string.IsNullOrEmpty(colorName) &&
                           string.IsNullOrEmpty(tagName) &&
                           string.IsNullOrEmpty(brandName)
                           ? await _productService.GetAllTakenAsync(6, 0)
                           : await _productService.FilterAsync(categoryName, colorName, tagName, brandName);

            if (minPrice.HasValue || maxPrice.HasValue)
            {
                products = products.Where(p =>
                    (!minPrice.HasValue || p.Price >= minPrice.Value) &&
                    (!maxPrice.HasValue || p.Price <= maxPrice.Value)).ToList();
            }

            if (!string.IsNullOrEmpty(sortType))
            {
                products = await _productService.GetSortedProductsAsync(sortType);
            }

            ViewBag.ProductsCount = productCount;
            ViewBag.CategoryProductCounts = categoryProductCounts;
            ViewBag.BrandProductCounts = brandProductCounts;

            ShopVM model = new ShopVM()
            {
                Categories = categories,
                Products = products,
                Brands = brands,
                Tags = tags,
                TotalProductCount = productCount,
                Setting = setting,
            };

            return View(model);
        }







        //[HttpGet]
        //public async Task<IActionResult> FilterByPricePartial(decimal? minPrice, decimal? maxPrice)
        //{
        //    var products = await _productService.FilterByPriceAsync(minPrice, maxPrice);
        //    return PartialView("_ProductPartial", products);
        //}


        [HttpGet]
        public async Task<IActionResult> ShowMore(int skip)
        {
            var products = await _productService.GetAllTakenAsync(6, skip);

            if (products == null || !products.Any())
            {
                return Json(new { success = false });
            }

            return PartialView("_ProductPartial", products);
        }
    }
}
