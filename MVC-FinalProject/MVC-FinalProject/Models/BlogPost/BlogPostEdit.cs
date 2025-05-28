namespace MVC_FinalProject.Models.BlogPost
{
    public class BlogPostEdit
    {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public string HighlightText { get; set; }
            public int BlogCategoryId { get; set; }
            public List<IFormFile> Images { get; set; } 
            public List<BlogPostImg> ExistingImages { get; set; } 
            public List<int> DeleteImageIds { get; set; } 
            public int? MainImageId { get; set; } 
     

    }
}
