using MVC_FinalProject.Helpers.Constants;
using MVC_FinalProject.Models.Review;
using MVC_FinalProject.Services.Interfaces;
using static MVC_FinalProject.Models.Review.ReviewEdit;

namespace MVC_FinalProject.Services
{
    public class ReviewService : IReviewService
    {
        private readonly HttpClient _httpClient;

        public ReviewService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Review> GetByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<Review>($"{Urls.ReviewClientUrl}{id}");
        }

        public async Task<HttpResponseMessage> CreateAsync(ReviewCreateApi model, string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"{Urls.ReviewClientUrl}CreateReview")
            {
                Content = JsonContent.Create(model)
            };

            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            return await _httpClient.SendAsync(request);
        }



        public async Task<HttpResponseMessage> EditAsync(ReviewEdit model, int id)
        {
            return await _httpClient.PutAsJsonAsync($"{Urls.ReviewClientUrl}{id}", model);
        }

        public async Task<HttpResponseMessage> DeleteAsync(int id)
        {
            return await _httpClient.DeleteAsync($"{Urls.ReviewClientUrl}{id}");
        }

        public Task<IEnumerable<Review>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Review>> GetByProductIdAsync(int productId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CanUserEditOrDelete(int reviewId, string userId)
        {
            throw new NotImplementedException();
        }


        public async Task<IEnumerable<Review>> GetAllByProductIdAsync(int productId)
        {
            var url = $"{Urls.ReviewClientUrl}GetAllByProductId?productId={productId}";

            try
            {
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"API Error: {response.StatusCode}, URL: {url}");
                    return Enumerable.Empty<Review>();
                }

                return await response.Content.ReadFromJsonAsync<IEnumerable<Review>>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}, URL: {url}");
                return Enumerable.Empty<Review>();
            }
        }

    }
}
