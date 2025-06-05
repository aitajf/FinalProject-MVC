using System.ComponentModel.DataAnnotations;

namespace MVC_FinalProject.Models.Product
{
    public class ProductEdit
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string Description { get; set; }
        [Required]
        public int BrandId { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public List<int> TagIds { get; set; }
        [Required]
        public List<int> ColorIds { get; set; }
        public List<IFormFile> UploadImages { get; set; } 
        public List<int> DeleteImageIds { get; set; } 
        public int? MainImageId { get; set; }
    }
}
