using System.Text.Json;
using MVC_FinalProject.Services.Interfaces;
using MVC_FinalProject.ViewModels;

namespace MVC_FinalProject.Services
{
    public class SettingService : ISettingService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl = "https://localhost:7004/api"; 

        public SettingService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<SettingVM>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync($"{_apiBaseUrl}/Setting/GetAll");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<IEnumerable<SettingVM>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
    }
}
