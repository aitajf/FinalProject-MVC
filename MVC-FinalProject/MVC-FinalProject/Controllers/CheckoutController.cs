using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;

namespace MVC_FinalProject.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly HttpClient _httpClient;
        public CheckoutController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(configuration["BaseUrl"]);

        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> OrderConfirmation()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Check([FromQuery] decimal data)
        {
            var domain = _httpClient.BaseAddress.ToString();

            var options = new SessionCreateOptions
            {
                SuccessUrl = domain + $"Home/",
                CancelUrl = domain + "home/index",
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
            };


            var sessionListItem = new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmount = (long)data * 100,
                    Currency = "usd",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = "JoiFurn"
                    },
                },
                Quantity = 1
            };
            options.LineItems.Add(sessionListItem);

            var service = new SessionService();
            Session session = service.Create(options);

            Response.Headers.Add("Location", session.Url);

            return new StatusCodeResult(303);
        }


        [HttpGet]
        public async Task<IActionResult> CheckOut([FromQuery] decimal data)
        {
            
            var domain = "https://localhost:7169/";

            var options = new SessionCreateOptions
            {
                SuccessUrl = domain + $"home/index",
                CancelUrl = domain + "home/index",
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
            };

            var sessionListItem = new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmount = (long)data * 100,
                    Currency = "usd",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = "JoiFurn"
                    },
                },
                Quantity = 1
            };
            options.LineItems.Add(sessionListItem);

            var service = new SessionService();
            Session session = service.Create(options);

            // 1. Token-dən userId çıxar
            string token = HttpContext.Session.GetString("AuthToken");
            string userid = string.Empty;
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            userid = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            // 2. Əvvəl səbəti sil
            await DeleteBasket(userid);

            // 3. Sonra cavab header-larını yaz və yönləndir
           
            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
        }

        //public async Task<IActionResult> DeleteProductsByUserId(string userId)
        //{
        //    try
        //    {
        //        var getResponse = await _httpClient.GetAsync($"api/Basket/GetBasketByUserId/{userId}");

        //        if (getResponse.IsSuccessStatusCode)
        //        {
        //            var content = await getResponse.Content.ReadAsStringAsync();
        //            var products = JsonConvert.DeserializeObject<List<BasketProduct>>(content);
        //            foreach (var product in products)
        //            {
        //                var deleteResponse = await _httpClient.DeleteAsync($"/api/ui/Basket/DeleteProduct/{(product.ProductId)}");

        //                if (!deleteResponse.IsSuccessStatusCode)
        //                {
        //                    return StatusCode((int)deleteResponse.StatusCode, "Some products not deleted.");
        //                }
        //            }
        //            return Ok("All product deleted.");
        //        }
        //        else
        //        {
        //            return StatusCode((int)getResponse.StatusCode, "Product not found.");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, "Server error.");
        //    }
        //}

        private async Task DeleteBasket(string userId)
        {
            var response = await _httpClient.DeleteAsync($"https://localhost:7004/api/Basket/DeleteBasketProduct/{userId}");

            if (!response.IsSuccessStatusCode) { }
        }
    }
}
