namespace MVC_FinalProject.Models.LandingBanner
{
    public class LandingBannerCreate
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public IFormFile Image { get; set; }
    }
}
