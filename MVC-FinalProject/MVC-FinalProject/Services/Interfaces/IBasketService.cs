using MVC_FinalProject.Models.Basket;

namespace MVC_FinalProject.Services.Interfaces
{
    public interface IBasketService
    {
        Task AddBasketAsync(BasketCreate basketCreate);
        Task<Basket> GetBasketByUserIdAsync(string userId);
        Task IncreaseQuantityAsync(BasketCreate basketCreate);
        Task DecreaseQuantityAsync(BasketCreate basketCreate);
    }
}
