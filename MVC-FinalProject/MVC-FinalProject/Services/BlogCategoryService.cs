using MVC_FinalProject.Helpers.Constants;
using MVC_FinalProject.Models.BlogCategory;
using MVC_FinalProject.Services.Interfaces;

namespace MVC_FinalProject.Services
{
    public class BlogCategoryService : IBlogCategoryService
    {
        private readonly HttpClient _httpClient;
        public BlogCategoryService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<HttpResponseMessage> CreateAsync(BlogCategoryCreate model)
        {
            return await _httpClient.PostAsJsonAsync($"{Urls.BlogCategoryUrl}Create", model);
        }

        public async Task<HttpResponseMessage> DeleteAsync(int id)
        {
            return await _httpClient.DeleteAsync($"{Urls.BlogCategoryUrl}Delete?id={id}");
        }

        public async Task<HttpResponseMessage> EditAsync(BlogCategoryEdit model, int id)
        {
            return await _httpClient.PutAsJsonAsync($"{Urls.BlogCategoryUrl}Edit/{id}", model);
        }

        public async Task<IEnumerable<BlogCategory>> GetAllAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<BlogCategory>>($"{Urls.BlogCategoryUrl}GetAll");
        }

        public async Task<BlogCategory> GetByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<BlogCategory>($"{Urls.BlogCategoryUrl}GetById/{id}");
        }
    }
}
