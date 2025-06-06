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

        public async Task<HttpResponseMessage> CreateAsync(ProductCreate model)
        {
            using var content = new MultipartFormDataContent();

            content.Add(new StringContent(model.Name), "Name");
            content.Add(new StringContent(model.Description), "Description");
            content.Add(new StringContent(model.Price.ToString()), "Price");
            content.Add(new StringContent(model.Stock.ToString()), "Stock");
            content.Add(new StringContent(model.CategoryId.ToString()), "CategoryId");
            content.Add(new StringContent(model.BrandId.ToString()), "BrandId");

            if (model.TagIds != null)
            {
                foreach (var tagId in model.TagIds)
                    content.Add(new StringContent(tagId.ToString()), "TagIds");
            }

            if (model.ColorIds != null)
            {
                foreach (var colorId in model.ColorIds)
                    content.Add(new StringContent(colorId.ToString()), "ColorIds");
            }


            if (model.Images != null)
            {
                foreach (var image in model.Images)
                {
                    using var ms = new MemoryStream();
                    await image.CopyToAsync(ms);
                    var fileBytes = ms.ToArray();

                    var byteContent = new ByteArrayContent(fileBytes);
                    byteContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
                    content.Add(byteContent, "Images", image.FileName);
                }
            }

            if (model.ColorImages != null)
            {
                foreach (var colorImage in model.ColorImages)
                {
                    using var ms = new MemoryStream();
                    await colorImage.CopyToAsync(ms);
                    var fileBytes = ms.ToArray();

                    var byteContent = new ByteArrayContent(fileBytes);
                    byteContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
                    content.Add(byteContent, "ColorImages", colorImage.FileName);
                }
            }

            return await _httpClient.PostAsync($"{Urls.ProductUrl}Create", content);
        }



        public async Task<HttpResponseMessage> EditAsync(int id, ProductEdit model)
        {
            using var content = new MultipartFormDataContent();

            content.Add(new StringContent(model.Name), "Name");
            content.Add(new StringContent(model.Description), "Description");
            content.Add(new StringContent(model.Price.ToString()), "Price");
            content.Add(new StringContent(model.Stock.ToString()), "Stock");
            content.Add(new StringContent(model.CategoryId.ToString()), "CategoryId");
            content.Add(new StringContent(model.BrandId.ToString()), "BrandId");

            if (model.TagIds != null)
            {
                foreach (var tagId in model.TagIds)
                    content.Add(new StringContent(tagId.ToString()), "TagIds");
            }

            if (model.ColorIds != null)
            {
                foreach (var colorId in model.ColorIds)
                    content.Add(new StringContent(colorId.ToString()), "ColorIds");
            }

            // Yeni şəkillər
            if (model.UploadImages != null)
            {
                foreach (var image in model.UploadImages)
                {
                    using var ms = new MemoryStream();
                    await image.CopyToAsync(ms);
                    var fileBytes = ms.ToArray();

                    var byteContent = new ByteArrayContent(fileBytes);
                    byteContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
                    content.Add(byteContent, "UploadImages", image.FileName);
                }
            }

            // Silinəcək şəkillərin Id-ləri
            if (model.DeleteImageIds != null)
            {
                foreach (var imgId in model.DeleteImageIds)
                    content.Add(new StringContent(imgId.ToString()), "DeleteImageIds");
            }

            // Əsas şəkil Id-si
            if (model.MainImageId.HasValue)
            {
                content.Add(new StringContent(model.MainImageId.Value.ToString()), "MainImageId");
            }

            return await _httpClient.PutAsync($"{Urls.ProductUrl}Edit/{id}", content);
        }





        public async Task<HttpResponseMessage> DeleteAsync(int id)
        {
            return await _httpClient.DeleteAsync($"{Urls.ProductUrl}Delete/{id}");
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<Product>($"{Urls.ProductUrl}GetById/{id}");
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<Product>>($"{Urls.ProductUrl}GetAll");
        }


        public async Task<IEnumerable<Product>> GetAllTakenAsync(int take, int? skip = null)
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<Product>>($"{Urls.ProductClientUrl}GetAllTaken/take?take={take}&skip={skip}");
        }

        public async Task<int> GetProductsCountAsync()
        {
            return await _httpClient.GetFromJsonAsync<int>($"{Urls.ProductClientUrl}GetProductsCount");
        }

        //public async Task<List<Product>> SearchByNameAsync(string? search)
        //{
        //    var searchQuery = search ?? string.Empty;
        //    var response = await _httpClient.GetAsync($"{Urls.ProductClientUrl}SearchByName?name={searchQuery}");

        //    if (!response.IsSuccessStatusCode) return new List<Product>();

        //    var json = await response.Content.ReadAsStringAsync();
        //    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        //    var result = JsonSerializer.Deserialize<List<Product>>(json, options);

        //    return result ?? new List<Product>();
        //}

    }
}
