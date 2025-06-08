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
        private  IBrandService _brandService;
        public ShopController(IProductService productService, 

            ICategoryService categoryService, IBrandService brandService, ITagService tagService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _brandService = brandService;
            _tagService = tagService;
        }

        public async Task<IActionResult> Index(string categoryName, string colorName, string tagName, string brandName, string sortType)
        {
            var categories = await _categoryService.GetAllAsync();
            var brands = await _brandService.GetAllAsync();
            var tags = await _tagService.GetAllAsync();
            var productCount = await _productService.GetProductsCountAsync();
            var categoryProductCounts = await _categoryService.GetCategoryProductCountsAsync();
            var brandProductCounts = await _brandService.GetBrandProductCountsAsync();

            var products = string.IsNullOrEmpty(categoryName) && string.IsNullOrEmpty(colorName) &&
                           string.IsNullOrEmpty(tagName) && string.IsNullOrEmpty(brandName)
                           ? await _productService.GetAllTakenAsync(6, 0)
                           : await _productService.FilterAsync(categoryName, colorName, tagName, brandName);

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
                TotalProductCount = productCount
            };

            return View(model);
        }




        public async Task<IActionResult> ShowMore(int skip)
        {
            var products = await _productService.GetAllAsync();
            var filteredProducts = await _productService.GetAllTakenAsync(8, skip);
            return PartialView("_ProductPartial", filteredProducts);
        }
    }
}
