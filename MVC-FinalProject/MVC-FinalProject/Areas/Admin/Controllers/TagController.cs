using Microsoft.AspNetCore.Mvc;
using MVC_FinalProject.Models.Tag;
using MVC_FinalProject.Services.Interfaces;

namespace MVC_FinalProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TagController : Controller
    {
        private readonly ITagService _tagService;
        public TagController(ITagService tagService)
        {
            _tagService = tagService;
        }
        public async Task<IActionResult> Index()
        {
            var res = await _tagService.GetAllAsync();
            return View(res);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TagCreate request)
        {

            var result = await _tagService.CreateAsync(request);
            if (result.IsSuccessStatusCode) return RedirectToAction(nameof(Index));
            ModelState.AddModelError(string.Empty, "Error creating");
            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var res = await _tagService.GetByIdAsync(id);
            if (res == null) return NotFound();
            return View(res);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _tagService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var res = await _tagService.GetByIdAsync(id);
            if (res == null) return NotFound();

            var bannerImg = new TagEdit
            {
                Id = id,
                Name = res.Name,
            };
            return View(bannerImg);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TagEdit request)
        {
            if (!ModelState.IsValid) return View(request);
            var result = await _tagService.EditAsync(request, id);
            if (result.IsSuccessStatusCode) return RedirectToAction(nameof(Index));
            ModelState.AddModelError("", "Error editing.");
            return View(request);
        }
    }
}
