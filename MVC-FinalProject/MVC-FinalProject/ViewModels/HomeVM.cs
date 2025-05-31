using MVC_FinalProject.Models.Category;
using MVC_FinalProject.Models.Instagram;
using MVC_FinalProject.Models.LandingBanner;
using MVC_FinalProject.Models.Product;
using MVC_FinalProject.Models.Slider;
using MVC_FinalProject.Models.SubscribeImg;

namespace MVC_FinalProject.ViewModels
{
    public class HomeVM
    {
        public IEnumerable<Slider> Sliders { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<LandingBanner> LandingBanners { get; set; }
        public IEnumerable<Instagram> Instagrams { get; set; }
        public IEnumerable<SubscribeImg> SubscribeImgs { get; set; }

    }
}
