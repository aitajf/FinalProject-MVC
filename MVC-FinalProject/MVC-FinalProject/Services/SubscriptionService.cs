using System.Text;
using MVC_FinalProject.Helpers.Constants;
using MVC_FinalProject.Models.Response;
using MVC_FinalProject.Models.Subscription;
using MVC_FinalProject.Services.Interfaces;
using Newtonsoft.Json;

namespace MVC_FinalProject.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public SubscriptionService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpContextAccessor = httpContextAccessor;
        }

        private void AddBearerToken()
        {
            var token = _httpContextAccessor.HttpContext?.Session.GetString("AuthToken");
            if (!string.IsNullOrEmpty(token))
            {
                if (_httpClient.DefaultRequestHeaders.Contains("Authorization"))
                    _httpClient.DefaultRequestHeaders.Remove("Authorization");

                _httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
        }

        public async Task<HttpResponseMessage> SubscribeAsync(SubscriptionCreate model)
        {
            AddBearerToken();
            return await _httpClient.PostAsJsonAsync($"{Urls.SubscriptionClientUrl}Subscribe", model);
        }

        public async Task<IEnumerable<Subscription>> GetAllSubscriptionsAsync()
        {
            AddBearerToken();
            return await _httpClient.GetFromJsonAsync<IEnumerable<Subscription>>($"{Urls.SubscriptionUrl}GetAllSubscriptions");
        }

        public async Task<HttpResponseMessage> UnsubscribeAsync(string email)
        {
            AddBearerToken();
            return await _httpClient.DeleteAsync($"{Urls.SubscriptionUrl}Unsubscribe?email={email}");
        }
    }
}
