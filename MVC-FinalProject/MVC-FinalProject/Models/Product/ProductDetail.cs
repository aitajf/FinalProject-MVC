using MVC_FinalProject.Models.Category;

namespace MVC_FinalProject.Models.Product
{
    public class ProductDetail
    {
        public int Id { get; set; }
        public int Stock { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string Brand { get; set; }
        public IEnumerable<ProductImage> ProductImages { get; set; }
        //public IEnumerable<Category> Categories { get; set; }
        //public IEnumerable<Tag> Tags { get; set; }
        //public IEnumerable<Color> Colors { get; set; }
    }
}
