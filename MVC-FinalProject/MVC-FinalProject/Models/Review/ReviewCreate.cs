using System.ComponentModel.DataAnnotations;

namespace MVC_FinalProject.Models.Review
{
    public class ReviewCreate
    {
        public int ProductId { get; set; }
        public string Comment { get; set; }
    }

    public class ReviewCreateApi
    {
        public int ProductId { get; set; }
        public string AppUserId { get; set; }
        public string Comment { get; set; }
    }
}
