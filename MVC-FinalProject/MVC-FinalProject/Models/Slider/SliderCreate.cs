using System.ComponentModel.DataAnnotations;

namespace MVC_FinalProject.Models.Slider
{
    public class SliderCreate
    {
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        public IFormFile Image { get; set; }
    }
}
