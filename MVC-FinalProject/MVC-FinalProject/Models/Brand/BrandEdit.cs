namespace MVC_FinalProject.Models.Brand
{
    public class BrandEdit
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public IFormFile Image { get; set; }
    }
}
