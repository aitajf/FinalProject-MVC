using MVC_FinalProject.Models.BlogCategory;
using MVC_FinalProject.Models.BlogPost;
using MVC_FinalProject.Models.Category;
using MVC_FinalProject.Models.Product;

namespace MVC_FinalProject.ViewModels
{
    public class BlogVM
    {
        public IEnumerable<BlogCategory> BlogCategories { get; set; }
        public IEnumerable<BlogPost> BlogPosts { get; set; }
    }
}
