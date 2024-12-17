using Restaurants.Application.Restaurants.DTOs;
using Restaurants.Domain.Entities;

namespace Restaurants.Application.Restaurants
{
    public interface IRestaurantsService
    {
        Task<IEnumerable<RestaurantDTO>> GetAllRestaurantsAsync();
        Task<RestaurantDTO?> GetByIdAsync(int id);
        Task<int> CreateAsync(CreateRestaurantDTO dto);
    }
}