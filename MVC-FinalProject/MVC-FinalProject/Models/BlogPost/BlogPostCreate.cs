using System.ComponentModel.DataAnnotations;

namespace MVC_FinalProject.Models.BlogPost
{
    public class BlogPostCreate
    {
        [Required]
        [RegularExpression(@"^(?=.*[A-Za-z])[A-Za-z0-9\s%\-_,.()!]+$", ErrorMessage = "Title must contain at least one letter and cannot be only numbers.")]
        public string Title { get; set; }
        [Required]
        [RegularExpression(@"^(?=.*[A-Za-z])[A-Za-z0-9\s%\-_,.()!]+$", ErrorMessage = "Description must contain at least one letter and cannot be only numbers.")]
        public string Description { get; set; }
        [Required]
        [RegularExpression(@"^(?=.*[A-Za-z])[A-Za-z0-9\s%\-_,.()!]+$", ErrorMessage = "Highlight text must contain at least one letter and cannot be only numbers.")]
        public string HighlightText { get; set; }
        [Required]
        public int BlogCategoryId { get; set; }
        [Required]
        public List<IFormFile> Images { get; set; }
    }
}
