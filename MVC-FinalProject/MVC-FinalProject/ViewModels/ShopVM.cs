using MVC_FinalProject.Models.Brand;
using MVC_FinalProject.Models.Category;
using MVC_FinalProject.Models.Product;

namespace MVC_FinalProject.ViewModels
{
    public class ShopVM
    {
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<Brand> Brands { get; set; }
        public int TotalProductCount { get; set; }
    }
}
