namespace MVC_FinalProject.Models.Category
{
    public class CategoryEdit
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public IFormFile Image { get; set; }
    }
}
