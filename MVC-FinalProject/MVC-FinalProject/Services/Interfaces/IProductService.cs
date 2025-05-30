using MVC_FinalProject.Helpers;
using MVC_FinalProject.Models.Product;

namespace MVC_FinalProject.Services.Interfaces
{
    public interface IProductService
    {
        Task<PaginationResponse<Product>> GetPaginatedProductsAsync(int page, int pageSize);
        //Task<Dictionary<string, object>> GetAllDropdownDataAsync();
        //Task<HttpResponseMessage> CreateAsync(ProductCreate model);
    }
}
