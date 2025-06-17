using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Text.Json;
using MVC_FinalProject.Helpers.Constants;
using MVC_FinalProject.Models.Basket;
using MVC_FinalProject.Services.Interfaces;

namespace MVC_FinalProject.Services
{
    public class BasketService : IBasketService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _contextAccessor;
        private const string BasketUrl = "https://localhost:7004/api/Basket/";

        public BasketService(HttpClient httpClient, IHttpContextAccessor contextAccessor)
        {
            _httpClient = httpClient;
            _contextAccessor = contextAccessor;
        }

        public async Task AddBasketAsync(BasketCreate basketCreate)
        {
            var token = _contextAccessor.HttpContext.Session.GetString("AuthToken");

            if (string.IsNullOrEmpty(token))
            {
                throw new UnauthorizedAccessException("Session expired. Please login again.");
            }

            var request = new HttpRequestMessage(HttpMethod.Post, BasketUrl + "AddBasket");
            request.Content = JsonContent.Create(basketCreate);
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Basket əlavə olunmadı: {error}");
            }
        }

        public async Task<Basket> GetBasketByUserIdAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentNullException(nameof(userId));

            var requestUrl = $"{BasketUrl}GetBasketByUserId/{userId}";

            var token = _contextAccessor.HttpContext.Session.GetString("AuthToken");
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }

            var response = await _httpClient.GetAsync(requestUrl);

            if (!response.IsSuccessStatusCode)
            {

                throw new Exception($"Failed: {response.StatusCode}");
            }

            var basket = await response.Content.ReadFromJsonAsync<Basket>();

            return basket;
        }


        public async Task IncreaseQuantityAsync(BasketCreate basketCreate)
        {
            var response = await _httpClient.PostAsJsonAsync(BasketUrl + "IncreaseQuantity", basketCreate);
            response.EnsureSuccessStatusCode();
        }

        public async Task DecreaseQuantityAsync(BasketCreate basketCreate)
        {
            var response = await _httpClient.PostAsJsonAsync(BasketUrl + "DecreaseQuantity", basketCreate);
            response.EnsureSuccessStatusCode();
        }
    }
}
