namespace MVC_FinalProject.Models.Review
{
    public class ProductReviewPage
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public List<string> ImageUrl { get; set; }  
        public List<string> Colors { get; set; } = new List<string>(); 
        public string Category { get; set; } 
        public string Description { get; set; } 
        public List<string> Tags { get; set; } = new List<string>(); 
        public decimal Price { get; set; }  
        public List<Review> Reviews { get; set; } = new List<Review>();
        public ReviewCreate NewReview { get; set; } 
    }

}
