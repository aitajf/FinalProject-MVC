using Microsoft.AspNetCore.Mvc;
using MVC_FinalProject.Services.Interfaces;

namespace MVC_FinalProject.Controllers
{
    public class ProductDetailController : Controller
	{
		private readonly IProductService _productService;
        public ProductDetailController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int id)
        {
            if (id == 0)
            {
                ModelState.AddModelError(string.Empty, "Invalid product ID.");
                return View("Index");
            }
            var product = await _productService.GetProductDetailAsync(id);
            if (product == null)
            {
                ModelState.AddModelError(string.Empty, "Product not found.");
                return View("Index");
            }
            return View(product);        
        }
    }
}

