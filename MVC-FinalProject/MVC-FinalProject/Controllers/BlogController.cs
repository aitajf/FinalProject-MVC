using Microsoft.AspNetCore.Mvc;
using MVC_FinalProject.Services.Interfaces;
using MVC_FinalProject.ViewModels;

namespace MVC_FinalProject.Controllers
{
    public class BlogController : Controller
    {
        private readonly IBlogCategoryService _categoryService;
        private readonly IBlogPostService _postService;
        public BlogController(IBlogCategoryService categoryService, 
                              IBlogPostService postService)
        {
            _categoryService = categoryService;
            _postService = postService;
        }
        public async Task<IActionResult> Index()
        {
            var categories = await _categoryService.GetAllAsync();
            var posts = await _postService.GetAllAsync();

            BlogVM model = new()
            {
                BlogCategories = categories,
                BlogPosts = posts
            };

            return View(model);
        }
    }
}
