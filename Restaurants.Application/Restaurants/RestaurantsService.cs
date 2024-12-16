using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;


namespace Restaurants.Application.Restaurants
{
    internal class RestaurantsService(IRestaurantsRepository restaurantsRepository, ILogger<RestaurantsService> logger) : IRestaurantsService
    {
        public async Task<IEnumerable<Restaurant>> GetAllRestaurantsAsync()
        {
            try
            {
                logger.LogInformation("Getting all restaurants");
                var restaurants = await restaurantsRepository.GetAllAsync();
                return restaurants;
            }
            catch (Exception e)
            {
                logger.LogInformation("Getting all restaurants error: "+e.Message);
                throw new Exception(e.Message);
            }
            
        }

        public Task<Restaurant> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
