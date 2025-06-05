using System.ComponentModel.DataAnnotations;

namespace MVC_FinalProject.Models.Review
{
    public class ReviewEdit
    {
            public int Id { get; set; }
            [Required(ErrorMessage = "Comment required")]
            [StringLength(2000, MinimumLength = 3)]
            public string Comment { get; set; }
            public int ProductId { get; set; }
    }
}
