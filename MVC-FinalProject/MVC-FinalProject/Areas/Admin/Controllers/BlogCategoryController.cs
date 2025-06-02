using Microsoft.AspNetCore.Mvc;
using MVC_FinalProject.Models.BlogCategory;
using MVC_FinalProject.Services.Interfaces;

namespace MVC_FinalProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BlogCategoryController : Controller
    {
        private readonly IBlogCategoryService _blogCategoryService;
        public BlogCategoryController(IBlogCategoryService blogCategoryService)
        {
            _blogCategoryService = blogCategoryService;
        }
        public async Task<IActionResult> Index()
        {
            var res = await _blogCategoryService.GetAllAsync();
            return View(res);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BlogCategoryCreate request)
        {

            var result = await _blogCategoryService.CreateAsync(request);
            if (result.IsSuccessStatusCode) return RedirectToAction(nameof(Index));
            ModelState.AddModelError(string.Empty, "Error creating");
            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var res = await _blogCategoryService.GetByIdAsync(id);
            if (res == null) return NotFound();
            return View(res);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _blogCategoryService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var res = await _blogCategoryService.GetByIdAsync(id);
            if (res == null) return NotFound();

            var bannerImg = new BlogCategoryEdit
            {
                Id = id,
                Name = res.Name,
            };
            return View(bannerImg);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BlogCategoryEdit request)
        {
            if (!ModelState.IsValid) return View(request);
            var result = await _blogCategoryService.EditAsync(request, id);
            if (result.IsSuccessStatusCode) return RedirectToAction(nameof(Index));
            ModelState.AddModelError("", "Error editing.");
            return View(request);
        }
    }
}
