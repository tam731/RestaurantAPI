using AutoMapper;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Restaurants.DTOs;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants
{
    internal class RestaurantsService(IRestaurantsRepository restaurantsRepository,
        ILogger<RestaurantsService> logger,
        IMapper mapper) : IRestaurantsService
    {
        public async Task<IEnumerable<RestaurantDTO>> GetAllRestaurantsAsync()
        {
            logger.LogInformation("Getting all restaurants");
            var restaurants = await restaurantsRepository.GetAllAsync();
            var restaurantsDTOs = mapper.Map<IEnumerable<RestaurantDTO>>(restaurants);
            return restaurantsDTOs;
        }

        public async Task<RestaurantDTO?> GetByIdAsync(int id)
        {
            try
            {
                var restaurant = await restaurantsRepository.GetByIdAsync(id);
                var restaurantDTO = mapper.Map<RestaurantDTO?>(restaurant);
                return restaurantDTO;
            }
            catch (Exception e)
            {
                logger.LogInformation("Get by id error: " + e.Message);
                throw new Exception(e.Message);
            }
        }

        public async Task<int> CreateAsync(CreateRestaurantDTO dto)
        {
            try
            {
                var restaurant = mapper.Map<Restaurant>(dto);

                int id = await restaurantsRepository.CreateAsync(restaurant);
                return id;
            }
            catch (Exception e)
            {
                logger.LogInformation("Create error: " + e.Message);
                throw new Exception(e.Message);
            }
        }
    }
}