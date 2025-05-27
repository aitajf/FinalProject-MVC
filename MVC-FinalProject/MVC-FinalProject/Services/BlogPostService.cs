using System.Net.Http.Headers;
using MVC_FinalProject.Helpers.Constants;
using MVC_FinalProject.Models.BlogCategory;
using MVC_FinalProject.Models.BlogPost;
using MVC_FinalProject.Services.Interfaces;

namespace MVC_FinalProject.Services
{
    public class BlogPostService : IBlogPostService
    {
        private readonly HttpClient _httpClient;
        public BlogPostService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<HttpResponseMessage> CreateAsync(BlogPostCreate model)
        {
            using (var multipartContent = new MultipartFormDataContent())
            {
                multipartContent.Add(new StringContent(model.Title), "Title");
                multipartContent.Add(new StringContent(model.Description), "Description");
                multipartContent.Add(new StringContent(model.HighlightText), "HighlightText");
                multipartContent.Add(new StringContent(model.BlogCategoryId.ToString()), "BlogCategoryId");
                if (model.Images != null && model.Images.Count > 0)
                {
                    foreach (var image in model.Images)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await image.CopyToAsync(memoryStream);
                            byte[] fileBytes = memoryStream.ToArray();

                            var fileContent = new ByteArrayContent(fileBytes);
                            fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
                            multipartContent.Add(fileContent, "Images", image.FileName);
                        }
                    }
                }
                return await _httpClient.PostAsync($"{Urls.BlogPostUrl}Create", multipartContent);
            }
        }

        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<BlogPost>>($"{Urls.BlogPostUrl}GetAll");
        }

        public async Task<BlogPost> GetByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<BlogPost>($"{Urls.BlogPostUrl}GetById/{id}");
        }

        public async Task<HttpResponseMessage> DeleteAsync(int id)
        {
            return await _httpClient.DeleteAsync($"{Urls.BlogPostUrl}Delete?id={id}");
        }

        public async Task<HttpResponseMessage> EditAsync(int id, BlogPostEdit model)
        {
            using var content = new MultipartFormDataContent();

            content.Add(new StringContent(model.Title), "Title");
            content.Add(new StringContent(model.Description), "Description");
            content.Add(new StringContent(model.HighlightText), "HighlightText");
            content.Add(new StringContent(model.BlogCategoryId.ToString()), "BlogCategoryId");

            if (model.Images != null)
            {
                foreach (var image in model.Images)
                {
                    using var ms = new MemoryStream();
                    await image.CopyToAsync(ms);
                    var fileContent = new ByteArrayContent(ms.ToArray());
                    fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
                    content.Add(fileContent, "Images", image.FileName);
                }
            }

            if (!string.IsNullOrEmpty(model.MainImageUrl))
                content.Add(new StringContent(model.MainImageUrl), "MainImageUrl");

            return await _httpClient.PutAsync($"{Urls.BlogPostUrl}Edit?id={id}", content);
        }

        public async Task<HttpResponseMessage> DeleteImageAsync(int blogPostId, int blogPostImageId)
        {
            return await _httpClient.DeleteAsync($"{Urls.BlogPostUrl}DeleteImage/{blogPostId}/{blogPostImageId}");
        }


    }
}
