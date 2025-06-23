namespace MVC_FinalProject.Models.PromoCode
{
    public class PromoCodeCreate
    {
        public string Code { get; set; }
        public int DiscountPercent { get; set; }
        public int UsageLimit { get; set; }
    }
}
