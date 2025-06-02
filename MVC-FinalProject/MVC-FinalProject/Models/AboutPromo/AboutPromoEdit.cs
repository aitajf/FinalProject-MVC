using System.ComponentModel.DataAnnotations;

namespace MVC_FinalProject.Models.AboutPromo
{
    public class AboutPromoEdit
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public IFormFile Image { get; set; }
    }
}
