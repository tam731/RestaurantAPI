using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;

namespace Restaurants.Domain.Interfaces
{
    public interface IRestaurantsAuthorizationService
    {
        bool Authorize(Restaurant restaurant, ResourceOperation operation);
    }
}