namespace MVC_FinalProject.Models.BlogPost
{
    public class BlogPostCreate
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string HighlightText { get; set; }
        public int BlogCategoryId { get; set; }
        public List<IFormFile> Images { get; set; }
    }
}
