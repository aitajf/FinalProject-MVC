using System.ComponentModel.DataAnnotations;

namespace MVC_FinalProject.Models.AboutPromo
{
    public class AboutPromoCreate
    {
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        public IFormFile Image { get; set; }
    }
}
