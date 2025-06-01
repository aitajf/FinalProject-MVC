using Microsoft.AspNetCore.Mvc;
using MVC_FinalProject.Services;
using MVC_FinalProject.Services.Interfaces;

namespace MVC_FinalProject.Controllers
{
    public class BlogDetailController : Controller
    {
        private readonly IBlogPostService _blogPostService;
        public BlogDetailController(IBlogPostService blogPostService)
        {
            _blogPostService = blogPostService;
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            if (id == 0)
            {
                ModelState.AddModelError(string.Empty, "Invalid ID.");
                return View("Index");
            }
            var post = await _blogPostService.GetByIdAsync(id);
            if (post == null)
            {
                ModelState.AddModelError(string.Empty, "Post not found.");
                return View("Index");
            }
            return View(post);
        }
    }
}
