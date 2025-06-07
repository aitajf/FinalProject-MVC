using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using MVC_FinalProject.Models.Product;
using MVC_FinalProject.Services.Interfaces;

namespace MVC_FinalProject.ViewComponents
{
    public class HeaderViewComponent : ViewComponent
    {
        private readonly IProductService _productService;

        public HeaderViewComponent(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string searchQuery)
        {
            var products = string.IsNullOrEmpty(searchQuery)
                ? new List<Product>()
                : await _productService.SearchByNameAsync(searchQuery);

            var searchResults = products.Select(p => new SearchProduct
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                BrandName = p.Brand , 
                CategoryName = p.Category
            }).ToList();

            return View(searchResults);
        }

    }

}
