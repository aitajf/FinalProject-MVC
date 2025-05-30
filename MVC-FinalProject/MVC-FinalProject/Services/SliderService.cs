using MVC_FinalProject.Helpers.Constants;
using MVC_FinalProject.Services.Interfaces;
using MVC_FinalProject.Models.Slider;
using System.Reflection.Metadata;
using System.Net.Http.Headers;
using System.Net.Http;
using MVC_FinalProject.Helpers;
using MVC_FinalProject.Models.LandingBanner;
using System.Text.Json;

namespace MVC_FinalProject.Services
{
    public class SliderService : ISliderService
    {
        private readonly HttpClient _httpClient;
        public SliderService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<HttpResponseMessage> CreateAsync(SliderCreate model)
        {
            using (var multipartContent = new MultipartFormDataContent())
            {
                // Add the fields from SliderCreateVM to the multipart content
                multipartContent.Add(new StringContent(model.Title), "Title");
                multipartContent.Add(new StringContent(model.Description), "Description");

                // Check if the file is provided in the form data
                if (model.Image != null && model.Image.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await model.Image.CopyToAsync(memoryStream);
                        byte[] fileBytes = memoryStream.ToArray();

                        var fileContent = new ByteArrayContent(fileBytes);
                        fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg"); // Set the correct media type if known
                        multipartContent.Add(fileContent, "Image", model.Image.FileName);
                    }
                }
                return await _httpClient.PostAsync($"{Urls.SliderUrl}Create", multipartContent);
            }
        }

        public async Task<HttpResponseMessage> DeleteAsync(int id)
        {
            return await _httpClient.DeleteAsync($"{Urls.SliderUrl}Delete?id={id}");
        }

        public async Task<HttpResponseMessage> EditAsync(SliderEdit model, int id)
        {

            using (var multipartContent = new MultipartFormDataContent())
            {
                // Add the fields from SliderCreateVM to the multipart content
                multipartContent.Add(new StringContent(model.Title), "Title");
                multipartContent.Add(new StringContent(model.Description), "Description");

                // Check if the file is provided in the form data
                if (model.Image != null && model.Image.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await model.Image.CopyToAsync(memoryStream);
                        byte[] fileBytes = memoryStream.ToArray();

                        var fileContent = new ByteArrayContent(fileBytes);
                        fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg"); // Set the correct media type if known
                        multipartContent.Add(fileContent, "Image", model.Image.FileName);
                    }
                }
                return await _httpClient.PutAsync($"{Urls.SliderUrl}Edit/{id}", multipartContent);
            }         
        }

        public async Task<IEnumerable<Slider>> GetAllAsync()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<Slider>>($"{Urls.SliderUrl}GetAll");
        }

        public async Task<Slider> GetByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<Slider>($"{Urls.SliderUrl}GetById/{id}");
        }

        public async Task<PaginationResponse<Slider>> GetPaginatedAsync(int page, int pageSize)
        {
            var response = await _httpClient.GetAsync($"{Urls.SliderUrl}GetPaginateDatas?page={page}&take={pageSize}");

            if (!response.IsSuccessStatusCode)
            {
                return PaginationResponse<Slider>.Create(new List<Slider>(), 0, page, pageSize);
            }

            var json = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var apiResponse = JsonSerializer.Deserialize<PaginationApiResponse<Slider>>(json, options);
            return PaginationResponse<Slider>.Create(apiResponse.Datas, apiResponse.TotalCount, apiResponse.CurrentPage, pageSize);
        }
    }
}
