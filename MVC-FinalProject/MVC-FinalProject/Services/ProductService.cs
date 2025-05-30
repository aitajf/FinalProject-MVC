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


        //public async Task<Dictionary<string, object>> GetAllDropdownDataAsync()
        //{
        //    var urls = new Dictionary<string, string>
        //    {
        //        { "brands", $"{Urls.BrandUrl}GetAll" },
        //        { "colors", $"{Urls.ColorUrl}GetAll" },
        //        { "tags", $"{Urls.TagUrl}GetAll" },
        //        { "categories", $"{Urls.CategoryUrl}GetAll" }
        //    };
        //    var results = new Dictionary<string, object>();
        //    foreach (var entry in urls)
        //    {
        //        var response = await _httpClient.GetAsync(entry.Value);
        //        if (response.IsSuccessStatusCode)
        //        {
        //            var json = await response.Content.ReadAsStringAsync();
        //            results[entry.Key] = JsonSerializer.Deserialize<object>(json);
        //        }
        //        else
        //        {
        //            results[entry.Key] = new List<object>(); 
        //        }
        //    }
        //    return results;
        //}

        public async Task<HttpResponseMessage> CreateAsync(ProductCreate model)
        {
            using var content = new MultipartFormDataContent();

            content.Add(new StringContent(model.Name), "Name");
            content.Add(new StringContent(model.Description), "Description");
            content.Add(new StringContent(model.Price.ToString()), "Price");
            content.Add(new StringContent(model.Stock.ToString()), "Stock");
            content.Add(new StringContent(model.CategoryId.ToString()), "CategoryId");
            content.Add(new StringContent(model.BrandId.ToString()), "BrandId");

            // Taglar
            if (model.TagIds != null)
            {
                foreach (var tagId in model.TagIds)
                    content.Add(new StringContent(tagId.ToString()), "TagIds");
            }

            // Rənglər
            if (model.ColorIds != null)
            {
                foreach (var colorId in model.ColorIds)
                    content.Add(new StringContent(colorId.ToString()), "ColorIds");
            }

            // Şəkillər
            if (model.UploadImages != null)
            {
                foreach (var image in model.UploadImages)
                {
                    using var ms = new MemoryStream();
                    await image.CopyToAsync(ms);
                    var fileBytes = ms.ToArray();

                    var byteContent = new ByteArrayContent(fileBytes);
                    byteContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
                    content.Add(byteContent, "Images", image.FileName);
                }
            }
            return await _httpClient.PostAsync($"{Urls.ProductUrl}Create", content);
        }
    }
}