using MVC_FinalProject.Helpers.Constants;
using MVC_FinalProject.Models.Subscription;
using System.Net.Http;
using MVC_FinalProject.Services.Interfaces;
using MVC_FinalProject.Models.AskUsFrom;

namespace MVC_FinalProject.Services
{
    public class AskUsFromService : IAskUsFromService
    {
        private readonly HttpClient _httpClient;
        public AskUsFromService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }
        public async Task<HttpResponseMessage> CreateQuestionAsync(AskUsFromCreate model)
        {
            return await _httpClient.PostAsJsonAsync($"{Urls.AskUsFromClientUrl}Create", model);
        }
    }
}
