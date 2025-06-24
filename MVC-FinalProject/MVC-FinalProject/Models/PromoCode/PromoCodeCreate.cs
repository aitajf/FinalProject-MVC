using System.ComponentModel.DataAnnotations;

namespace MVC_FinalProject.Models.PromoCode
{
    public class PromoCodeCreate
    {
        [Required]
        public string Code { get; set; }
        [Required]

        public int DiscountPercent { get; set; }
        [Required]
        public int UsageLimit { get; set; }
    }
}
