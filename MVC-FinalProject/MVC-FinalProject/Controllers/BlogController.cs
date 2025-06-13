using Microsoft.AspNetCore.Mvc;
using MVC_FinalProject.Services;
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
        //public async Task<IActionResult> Index(int page = 1, int pageSize = 3)
        //{
        //    var categories = await _categoryService.GetAllAsync();
        //    var paginate = await _postService.GetPaginatedAsync(page, pageSize);

        //    var categoryPostCounts = await _categoryService.GetCategoryPostCountsAsync();

        //    ViewBag.CategoryPostCounts = categoryPostCounts;

        //    BlogVM model = new()
        //    {
        //        BlogCategories = categories,
        //        Paginate = paginate
        //    };

        //    return View(model);
        //}

        //[HttpGet]
        //public async Task<IActionResult> Search(string name)
        //{
        //    var categories = await _categoryService.GetAllAsync();
        //    var results = await _postService.SearchByCategoryAndName(name);

        //    BlogVM model = new()
        //    {
        //        BlogCategories = categories,
        //        SearchResults = results,
        //        SearchText = name
        //    };

        //    return View("Index", model);
        //}


        public async Task<IActionResult> Index(int page = 1, int pageSize = 3)
        {
            var categories = await _categoryService.GetAllAsync();
            var paginate = await _postService.GetPaginatedAsync(page, pageSize);
            var categoryPostCounts = await _categoryService.GetCategoryPostCountsAsync();

            ViewBag.CategoryPostCounts = categoryPostCounts;

            BlogVM model = new()
            {
                BlogCategories = categories,
                Paginate = paginate
            };

            // AJAX gəlibsə, sadəcə content hissəsini qayıt
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                return PartialView("_BlogSearchPartial", model);

            return View(model);
        }







        public async Task<IActionResult> Search(string name)
        {
            var results = await _postService.SearchByCategoryAndName(name);
            var categories = await _categoryService.GetAllAsync();
            var categoryPostCounts = await _categoryService.GetCategoryPostCountsAsync();

            ViewBag.CategoryPostCounts = categoryPostCounts;

            var model = new BlogVM
            {
                SearchText = name,
                SearchResults = results,
                BlogCategories = categories
            };

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                return PartialView("_BlogSearchPartial", model);

            return View("Index", model);
        }


    }
}
