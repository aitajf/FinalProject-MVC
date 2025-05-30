using Microsoft.AspNetCore.Mvc;
using MVC_FinalProject.Services.Interfaces;

namespace MVC_FinalProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        public async Task<IActionResult> Index(int page = 1, int pageSize = 4)
        {
            var paginatedProducts = await _productService.GetPaginatedProductsAsync(page, pageSize);
            return View(paginatedProducts);
        }     
    }
}