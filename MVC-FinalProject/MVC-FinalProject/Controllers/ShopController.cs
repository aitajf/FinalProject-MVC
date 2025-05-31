using Microsoft.AspNetCore.Mvc;
using MVC_FinalProject.Services.Interfaces;
using MVC_FinalProject.ViewModels;

namespace MVC_FinalProject.Controllers
{
    public class ShopController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private  IBrandService _brandService;
        public ShopController(IProductService productService, ICategoryService categoryService, IBrandService brandService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _brandService = brandService;
        }
        public async Task<IActionResult> Index()
        {
            var categories = await _categoryService.GetAllAsync();
            var brands = await _brandService.GetAllAsync();
            var products = await _productService.GetAllTakenAsync(8, 0);
            var productCount = await _productService.GetProductsCountAsync();

            ViewBag.ProductsCount = productCount;

            ShopVM model = new ShopVM()
            {
                Categories = categories,
                Products = products,
                Brands = brands,
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
