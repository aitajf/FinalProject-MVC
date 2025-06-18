using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using MVC_FinalProject.Models.Product;
using MVC_FinalProject.Services.Interfaces;
using MVC_FinalProject.ViewModels;

namespace MVC_FinalProject.ViewComponents
{
    public class HeaderViewComponent : ViewComponent
    {
        private readonly IProductService _productService;
        private readonly ISettingService _settingService;

        public HeaderViewComponent(IProductService productService, ISettingService settingService)
        {
            _productService = productService;
            _settingService = settingService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string searchQuery)
        {
            var settings = await _settingService.GetAllAsync();   

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

            HeaderVMVC model = new()
            {
                Setting = settings,
                SearchProducts = searchResults,

            };
            return View(model);
        }

        public class HeaderVMVC { 
            public IEnumerable<SearchProduct> SearchProducts { get; set; }
            public IEnumerable<SettingVM>  Setting { get; set; }
        }

    }

}
