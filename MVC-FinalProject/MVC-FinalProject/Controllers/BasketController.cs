using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC_FinalProject.Models.Basket;
using MVC_FinalProject.Services.Interfaces;

namespace MVC_FinalProject.Controllers
{
    public class BasketController : Controller
    {
        private readonly IBasketService _basketService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BasketController(IBasketService basketService, IHttpContextAccessor httpContextAccessor)
        {
            _basketService = basketService;
            _httpContextAccessor = httpContextAccessor; 
        }

        public async Task<IActionResult> Index()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            var basket = await _basketService.GetBasketByUserIdAsync(userId);

            return View(basket);

        }

        [Authorize(Roles = "Admin,SuperAdmin,Member")]
        [HttpPost]
        public async Task<IActionResult> AddToBasket(int productId, int colorId)
        {
            var token = HttpContext.Session.GetString("AuthToken");
            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized();
            }

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var basketDto = new BasketCreate
            {
                UserId = userId,
                ProductId = productId,
                ColorId = colorId
            };

            try
            {
                await _basketService.AddBasketAsync(basketDto);
                return RedirectToAction("Index", "Home"); // və ya məhsul səhifəsinə geri
            }
            catch (Exception ex)
            {
                TempData["BasketError"] = ex.Message;
                return RedirectToAction("Detail", "Product", new { id = productId });
            }
        }

            [HttpPost]
        public async Task<IActionResult> IncreaseQuantity([FromBody] BasketCreate basketCreate)
        {
            try
            {
                await _basketService.IncreaseQuantityAsync(basketCreate);
                return Ok(new { success = true, message = "Quantity increased." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DecreaseQuantity([FromBody] BasketCreate basketCreate)
        {
            try
            {
                await _basketService.DecreaseQuantityAsync(basketCreate);
                return Ok(new { success = true, message = "Quantity decreased." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

    }
}
