using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVC_FinalProject.Models.Brand;
using MVC_FinalProject.Models.Category;
using MVC_FinalProject.Models.Color;
using MVC_FinalProject.Models.Product;
using MVC_FinalProject.Models.Tag;
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


        //private async Task SetViewBagData()
        //{
        //    var dropdownData = await _productService.GetAllDropdownDataAsync();

        //    var brands = JsonSerializer.Deserialize<List<Brand>>(dropdownData["brands"].ToString());
        //    var colors = JsonSerializer.Deserialize<List<Color>>(dropdownData["colors"].ToString());
        //    var tags = JsonSerializer.Deserialize<List<Tag>>(dropdownData["tags"].ToString());
        //    var categories = JsonSerializer.Deserialize<List<Category>>(dropdownData["categories"].ToString());

        //    ViewBag.brands = new SelectList(brands, "id", "name");
        //    ViewBag.colors = new MultiSelectList(colors, "id", "name");
        //    ViewBag.tags = new MultiSelectList(tags, "id", "name");
        //    ViewBag.categories = new SelectList(categories, "id", "name");
        //}

        //private async Task SetViewBagData()
        //{
        //    var dropdownData = await _productService.GetAllDropdownDataAsync();

        //    var brands = dropdownData.ContainsKey("brands") && dropdownData["brands"] != null
        //        ? JsonSerializer.Deserialize<List<Brand>>(dropdownData["brands"].ToString())
        //        : new List<Brand>();

        //    var colors = dropdownData.ContainsKey("colors") && dropdownData["colors"] != null
        //        ? JsonSerializer.Deserialize<List<Color>>(dropdownData["colors"].ToString())
        //        : new List<Color>();

        //    var tags = dropdownData.ContainsKey("tags") && dropdownData["tags"] != null
        //        ? JsonSerializer.Deserialize<List<Tag>>(dropdownData["tags"].ToString())
        //        : new List<Tag>();

        //    var categories = dropdownData.ContainsKey("categories") && dropdownData["categories"] != null
        //        ? JsonSerializer.Deserialize<List<Category>>(dropdownData["categories"].ToString())
        //        : new List<Category>();

        //    ViewBag.brands = brands.Any() ? new SelectList(brands, "id", "name") : new SelectList(new List<Brand>(), "id", "name");
        //    ViewBag.colors = colors.Any() ? new MultiSelectList(colors, "id", "name") : new MultiSelectList(new List<Color>(), "id", "name");
        //    ViewBag.tags = tags.Any() ? new MultiSelectList(tags, "id", "name") : new MultiSelectList(new List<Tag>(), "id", "name");
        //    ViewBag.categories = categories.Any() ? new SelectList(categories, "id", "name") : new SelectList(new List<Category>(), "id", "name");
        //}




        //[HttpGet]
        //public async Task<IActionResult> Create()
        //{
        //    await SetViewBagData();
        //    return View();
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create(ProductCreate postProductVM)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        await SetViewBagData(); 
        //        return View(postProductVM);
        //    }

        //    var response = await _productService.CreateAsync(postProductVM);

        //    if (response.IsSuccessStatusCode)
        //    {
        //        return RedirectToAction("Index");
        //    }
        //    else
        //    {
        //        ModelState.AddModelError(string.Empty, "Something wrong.");
        //        await SetViewBagData(); 
        //        return View(postProductVM);
        //    }
        //}
    }
}