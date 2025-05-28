using MVC_FinalProject.Models.BlogCategory;
using MVC_FinalProject.Models.BlogPost;
using MVC_FinalProject.Models.Brand;

namespace MVC_FinalProject.Services.Interfaces
{
    public interface IBlogPostService
    {
        Task<HttpResponseMessage> CreateAsync(BlogPostCreate model);
        Task<IEnumerable<BlogPost>> GetAllAsync();
        Task<BlogPost> GetByIdAsync(int id);
        Task<HttpResponseMessage> DeleteAsync(int id);
        Task<HttpResponseMessage> EditAsync(int id, BlogPostEdit model, List<int>? imagesToDelete, int? mainImageId);
    }
}
