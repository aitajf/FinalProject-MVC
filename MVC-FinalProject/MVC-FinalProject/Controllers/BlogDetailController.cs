using System.Net;
using Microsoft.AspNetCore.Mvc;
using MVC_FinalProject.Models.BlogReview;
using MVC_FinalProject.Models.Product;
using MVC_FinalProject.Models.Review;
using MVC_FinalProject.Services;
using MVC_FinalProject.Services.Interfaces;

namespace MVC_FinalProject.Controllers
{
    public class BlogDetailController : Controller
    {
        private readonly IBlogPostService _blogPostService;
        private readonly IBlogReviewService _blogReviewService;
        public BlogDetailController(IBlogPostService blogPostService,
                                    IBlogReviewService blogReviewService)
        {
            _blogPostService = blogPostService;
            _blogReviewService = blogReviewService;
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id, int? editingReviewId = null)
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
            var reviews = await _blogReviewService.GetAllByPostIdAsync(id);


            var viewModel = new PostReviewPage
            {
                PostId = post.Id,
                PostName = post.Title,
                BlogPostImgs = post.Images,
                BlogCategory = post.BlogCategory,
                BlogReviews = reviews.ToList(),
                Description = post.Description,
                HighlightText = post.HighlightText,
                NewReview = new BlogReviewCreate { PostId = id }
            };

            ViewData["EditingReviewId"] = editingReviewId;
            return View(viewModel);
        }



        [HttpPost]
        public async Task<IActionResult> AddReview(BlogReviewCreate model)
        {
            if (!ModelState.IsValid)
            {
                var post = await _blogPostService.GetByIdAsync(model.PostId);
                var reviews = await _blogReviewService.GetAllByPostIdAsync(model.PostId);

                var vm = new PostReviewPage
                {
                    PostId = post.Id,
                    PostName = post.Title,
                    Title = post.Title,
                    HighlightText = post.HighlightText,
                    BlogCategory = post.BlogCategory,
                    BlogPostImgs = post.Images,
                    Description = post.Description,
                    BlogReviews = reviews.ToList(),
                    NewReview = model
                };

                return View("Detail", vm);
            }


            var token = HttpContext.Session.GetString("AuthToken")
           ?? User.Claims.FirstOrDefault(c => c.Type == "access_token")?.Value;
            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login", "Account");


            var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User ID not found in token.");

            var reviewDto = new BlogReviewCreateApi
            {
                PostId = model.PostId,
                Comment = model.Comment,
                AppUserId = userId
            };

            var response = await _blogReviewService.CreateAsync(reviewDto, token);

            if (!response.IsSuccessStatusCode)
                return BadRequest("Failed to add review.");

            return RedirectToAction("Detail", new { id = model.PostId });
        }

        [HttpGet]
        public async Task<IActionResult> EditReview(int id)
        {
            var reviewDto = await _blogReviewService.GetByIdAsync(id);
            if (reviewDto == null)
                return NotFound();

            var model = new BlogReviewEdit
            {
                PostId = reviewDto.PostId,
                AppUserId = reviewDto.AppUserId,
                Comment = reviewDto.Comment
            };

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> EditReview(int id, BlogReviewEdit model)
        {
            var token = HttpContext.Session.GetString("AuthToken");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Account");
            }
            var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;


            if (!string.Equals(userId, model.AppUserId, StringComparison.OrdinalIgnoreCase))
            {
                return Forbid("User ID does not match the review owner.");
            }


            if (!ModelState.IsValid)
                return View(model);


            var dto = new BlogReviewEditApi
            {
                AppUserId = userId,
                Comment = model.Comment
            };

            var response = await _blogReviewService.EditAsync(dto, id);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Detail", new { id = model.PostId });
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                ModelState.AddModelError("", "You are not authorized to edit this review.");
                return View(model);
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return NotFound();
            }

            ModelState.AddModelError("", "An error occurred while editing the review.");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteReview(int id, int postId)
        {
            var token = HttpContext.Session.GetString("AuthToken");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Account");
            }

            var response = await _blogReviewService.DeleteAsync(id, token);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Detail", new { id = postId });
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                TempData["Error"] = "You are not authorized to delete this review.";
            }
            else
            {
                TempData["Error"] = "An error occurred while deleting the review.";
            }

            return RedirectToAction("Detail", new { id = postId });
        }
    }
}
