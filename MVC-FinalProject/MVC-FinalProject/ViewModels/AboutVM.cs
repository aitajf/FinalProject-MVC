using MVC_FinalProject.Models.AboutBannerImg;
using MVC_FinalProject.Models.Brand;
using MVC_FinalProject.Models.SubscribeImg;

namespace MVC_FinalProject.ViewModels
{
    public class AboutVM
    {
        public IEnumerable<AboutBannerImg> AboutBannerImgs { get; set; }
        public IEnumerable<Brand> Brands { get; set; }
    }
}
