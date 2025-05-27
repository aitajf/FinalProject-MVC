using System.ComponentModel.DataAnnotations;

namespace MVC_FinalProject.Models.Tag
{
    public class TagEdit
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
