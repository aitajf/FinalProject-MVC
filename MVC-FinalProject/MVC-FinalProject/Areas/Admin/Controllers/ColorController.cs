using Microsoft.AspNetCore.Mvc;
using MVC_FinalProject.Models.Color;
using MVC_FinalProject.Models.Instagram;
using MVC_FinalProject.Services;
using MVC_FinalProject.Services.Interfaces;

namespace MVC_FinalProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ColorController : Controller
    {
        private readonly IColorService _colorService;
        public ColorController(IColorService colorService)
        {
            _colorService = colorService;
        }
        public async Task<IActionResult> Index()
        {
            var res = await _colorService.GetAllAsync();
            return View(res);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ColorCreate request)
        {

            var result = await _colorService.CreateAsync(request);
            if (result.IsSuccessStatusCode) return RedirectToAction(nameof(Index));
            ModelState.AddModelError(string.Empty, "Error creating");
            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var res = await _colorService.GetByIdAsync(id);
            if (res == null) return NotFound();
            return View(res);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _colorService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var res = await  _colorService.GetByIdAsync(id);
            if (res == null) return NotFound();

            var bannerImg = new ColorEdit
            {
                Id = id,
               Name = res.Name,
            };
            return View(bannerImg);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ColorEdit request)
        {
            if (!ModelState.IsValid) return View(request);
            var result = await _colorService.EditAsync(request, id);
            if (result.IsSuccessStatusCode) return RedirectToAction(nameof(Index));
            ModelState.AddModelError("", "Error editing.");
            return View(request);
        }
    }
}
