using MVC_FinalProject.Models.Category;
using MVC_FinalProject.Models.Slider;

namespace MVC_FinalProject.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<HttpResponseMessage> CreateAsync(CategoryCreate model);
        Task<HttpResponseMessage> EditAsync(CategoryEdit model, int id);
        Task<HttpResponseMessage> DeleteAsync(int id);
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category> GetByIdAsync(int id);
    }
}
