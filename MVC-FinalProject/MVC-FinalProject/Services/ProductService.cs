using MVC_FinalProject.Helpers.Constants;
using System.Text.Json;
using MVC_FinalProject.Services.Interfaces;
using MVC_FinalProject.Models.Product;
using MVC_FinalProject.Helpers;
using System.Net.Http.Headers;

namespace MVC_FinalProject.Services
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _httpClient;

        public ProductService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<PaginationResponse<Product>> GetPaginatedProductsAsync(int page, int pageSize)
        {
            var response = await _httpClient.GetAsync($"{Urls.ProductUrl}GetPaginateDatas?page={page}&take={pageSize}");

            if (!response.IsSuccessStatusCode)
            {
                return PaginationResponse<Product>.Create(new List<Product>(), 0, page, pageSize);
            }

            var json = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var apiResponse = JsonSerializer.Deserialize<PaginationApiResponse<Product>>(json, options);
            return PaginationResponse<Product>.Create(apiResponse.Datas, apiResponse.TotalCount, apiResponse.CurrentPage, pageSize);
        }


        public async Task<Dictionary<string, object>> GetAllDropdownDataAsync()
        {
            var urls = new Dictionary<string, string>
            {
                { "brands", $"{Urls.BrandUrl}GetAll" },
                { "colors", $"{Urls.ColorUrl}GetAll" },
                { "tags", $"{Urls.TagUrl}GetAll" },
                { "categories", $"{Urls.CategoryUrl}GetAll" }
            };
            var results = new Dictionary<string, object>();
            foreach (var entry in urls)
            {
                var response = await _httpClient.GetAsync(entry.Value);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    results[entry.Key] = JsonSerializer.Deserialize<object>(json);
                }
                else
                {
                    results[entry.Key] = new List<object>(); 
                }
            }
            return results;
        }

        public async Task<HttpResponseMessage> CreateAsync(ProductCreate model)
        {
            var dropdownData = await GetAllDropdownDataAsync();

            using (var multipartContent = new MultipartFormDataContent())
            {
                multipartContent.Add(new StringContent(model.Name), "Name");
                multipartContent.Add(new StringContent(model.Price.ToString()), "Price");
                multipartContent.Add(new StringContent(model.Stock.ToString()), "Stock");
                multipartContent.Add(new StringContent(model.Description), "Description");
                multipartContent.Add(new StringContent(model.BrandId.ToString()), "BrandId");
                multipartContent.Add(new StringContent(model.CategoryId.ToString()), "CategoryId");

                foreach (var tagId in model.TagIds)
                {
                    multipartContent.Add(new StringContent(tagId.ToString()), "TagIds");
                }

                foreach (var colorId in model.ColorIds)
                {
                    multipartContent.Add(new StringContent(colorId.ToString()), "ColorIds");
                }

                if (model.UploadImages != null && model.UploadImages.Any())
                {
                    foreach (var file in model.UploadImages)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await file.CopyToAsync(memoryStream);
                            var fileContent = new ByteArrayContent(memoryStream.ToArray());
                            fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse(file.ContentType);
                            multipartContent.Add(fileContent, "UploadImages", file.FileName);
                        }
                    }
                }
                return await _httpClient.PostAsync($"{Urls.ProductUrl}Create", multipartContent);
            }
        }

    }
}
