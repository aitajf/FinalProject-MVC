using System.ComponentModel.DataAnnotations;

namespace MVC_FinalProject.Models.Slider
{
    public class SliderEdit
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
