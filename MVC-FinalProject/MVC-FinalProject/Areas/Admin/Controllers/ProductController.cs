using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVC_FinalProject.Models.Product;
using MVC_FinalProject.Services.Interfaces;

namespace MVC_FinalProject.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin")]
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly IColorService _colorService;
        private readonly ITagService _tagService;
        private readonly ICategoryService _categoryService;
        private readonly IBrandService _brandService;

        public ProductController(
            IProductService productService,
            IColorService colorService,
            ITagService tagService,
            ICategoryService categoryService,
            IBrandService brandService)
        {
            _productService = productService;
            _colorService = colorService;
            _tagService = tagService;
            _categoryService = categoryService;
            _brandService = brandService;
        }

        private async Task PopulateDropdownsAsync()
        {
            var colors = await _colorService.GetAllAsync();
            var tags = await _tagService.GetAllAsync();
            var brands = await _brandService.GetAllAsync();
            var categories = await _categoryService.GetAllAsync();

            ViewBag.Colors = colors.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList();

            ViewBag.Tags = tags.Select(t => new SelectListItem
            {
                Value = t.Id.ToString(),
                Text = t.Name
            }).ToList();

            ViewBag.Brands = brands.Select(b => new SelectListItem
            {
                Value = b.Id.ToString(),
                Text = b.Name
            }).ToList();

            ViewBag.Categories = categories.Select(cat => new SelectListItem
            {
                Value = cat.Id.ToString(),
                Text = cat.Name
            }).ToList();
        }


        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, int pageSize = 4)
        {
            var paginatedProducts = await _productService.GetPaginatedProductsAsync(page, pageSize);
            ViewBag.TotalCount =paginatedProducts.TotalCount;

            return View(paginatedProducts);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await PopulateDropdownsAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCreate model)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropdownsAsync(); 
                return View(model);
            }
            var response = await _productService.CreateAsync(model);

            if (response.IsSuccessStatusCode) return RedirectToAction(nameof(Index));
            ModelState.AddModelError("", "Product creation failed.");
            await PopulateDropdownsAsync();
            return View(model);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product == null) return NotFound();

            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _productService.DeleteAsync(id);
            if (response.IsSuccessStatusCode)
            {
                return Ok();
            }          
            return RedirectToAction("Index");
        }

        //[HttpGet]
        //public async Task<IActionResult> Edit(int id)
        //{
        //    var product = await _productService.GetByIdAsync(id);
        //    if (product == null)
        //        return NotFound();

        //    var model = new ProductEdit
        //    {
        //        Id = product.Id,
        //        Name = product.Name,
        //        Description = product.Description,
        //        Price = product.Price,
        //        Stock = product.Stock,
        //        CategoryId = product.Category,
        //        BrandId = product.BrandId,
        //        ColorIds = product.P.Select(pc => pc.ColorId).ToList(),
        //        TagIds = product.ProductTags.Select(pt => pt.TagId).ToList(),
        //        ExistingImages = product.Images.Select(pi => new ProductImage
        //        {
        //            Id = pi.Id,
        //            ImgUrl = pi.Img,
        //            IsMain = pi.IsMain
        //        }).ToList()
        //    };

        //    await PopulateDropdownsAsync();
        //    return View(model);
        //}

    }
}