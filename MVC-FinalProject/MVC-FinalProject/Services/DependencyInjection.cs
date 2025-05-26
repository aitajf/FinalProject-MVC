using MVC_FinalProject.Services;
using MVC_FinalProject.Services.Interfaces;

namespace Service
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServiceLayer(this IServiceCollection services)
        {         
            services.AddScoped<ISliderService, SliderService>();         
            services.AddScoped<ICategoryService, CategoryService>();         
            services.AddScoped<IAboutBannerImgService, AboutBannerImgService >();         
            services.AddScoped<ILandingBannerService, LandingBannerService>();         
            services.AddScoped<IInstagramService, InstagramService>();         
            services.AddScoped<ISubscribeImgService, SubscribeImgService>();         
            return services;
        }
    }
}