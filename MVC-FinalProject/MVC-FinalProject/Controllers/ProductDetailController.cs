using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVC_FinalProject.Models.Review;
using MVC_FinalProject.Services.Interfaces;

namespace MVC_FinalProject.Controllers
{
    public class ProductDetailController : Controller
	{
		private readonly IProductService _productService;
        private readonly IReviewService _reviewService;

        public ProductDetailController(IProductService productService,
                                       IReviewService reviewService)
        {
            _productService = productService;
            _reviewService = reviewService;
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id, int? editingReviewId = null)
        {
            if (id == 0)
            {
                ModelState.AddModelError(string.Empty, "Invalid product ID.");
                return View("Index");
            }

            var product = await _productService.GetByIdAsync(id);
            if (product == null)
            {
                ModelState.AddModelError(string.Empty, "Product not found.");
                return View("Index");
            }

            var reviews = await _reviewService.GetAllByProductIdAsync(id);

            var viewModel = new ProductReviewPage
            {
                ProductId = product.Id,
                ProductName = product.Name,
                ImageUrl = product.Images,
                Colors = product.Colors.ToList(),
                Category = product.Category,
                Tags = product.Tags.ToList(),
                Price = product.Price,
                Reviews = reviews.ToList(),
                Description = product.Description,
                NewReview = new ReviewCreate { ProductId = id }
            };

            ViewData["EditingReviewId"] = editingReviewId;
            return View(viewModel);
        }


        [HttpPost]
        public async Task<IActionResult> AddReview(ReviewCreate model)
        {
            if (!ModelState.IsValid)
            {
                var product = await _productService.GetByIdAsync(model.ProductId);
                var reviews = await _reviewService.GetAllByProductIdAsync(model.ProductId);

                var vm = new ProductReviewPage
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    ImageUrl = product.Images,
                    Colors = product.Colors.ToList(),
                    Category = product.Category,
                    Tags = product.Tags.ToList(),
                    Price = product.Price,
                    Description = product.Description,
                    Reviews = reviews.ToList(),
                    NewReview = model
                };

                return View("Detail", vm);
            }

            var token = HttpContext.Session.GetString("AuthToken")
           ?? User.Claims.FirstOrDefault(c => c.Type == "access_token")?.Value;

            if (string.IsNullOrEmpty(token))
                return Unauthorized("Token missing!");

            var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User ID not found in token.");

            var reviewDto = new ReviewCreateApi
            {
                ProductId = model.ProductId,
                Comment = model.Comment,
                AppUserId = userId
            };

            var response = await _reviewService.CreateAsync(reviewDto, token);

            if (!response.IsSuccessStatusCode)
                return BadRequest("Failed to add review.");

            return RedirectToAction("Detail", new { id = model.ProductId });
        }

        [HttpGet]
        public async Task<IActionResult> EditReview(int id)
        {
            var reviewDto = await _reviewService.GetByIdAsync(id);
            if (reviewDto == null)
                return NotFound();

            var model = new ReviewEdit
            {
                ProductId = reviewDto.ProductId,
                AppUserId = reviewDto.AppUserId,
                Comment = reviewDto.Comment
            };

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> EditReview(int id, ReviewEdit model)
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


              var dto = new ReviewEditApi
              {
                  AppUserId = userId,
                  Comment = model.Comment
              };

            var response = await _reviewService.EditAsync(dto, id);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Detail", new { id = model.ProductId });
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





        //[Authorize]
        [HttpPost]
        public async Task<IActionResult> DeleteReview(int id)
        {
            var response = await _reviewService.DeleteAsync(id);
            if (!response.IsSuccessStatusCode)
                return BadRequest("Failed to delete review.");

            return Redirect(Request.Headers["Referer"].ToString());
        }

    }
}

