using Microsoft.Extensions.Logging;
using Restaurants.Application.Users;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Interfaces;

namespace Restaurants.Infrastructure.Authorization.Services;

public class RestaurantsAuthorizationService(ILogger<RestaurantsAuthorizationService> logger,
    IUserContext userContext) : IRestaurantsAuthorizationService
{
    public bool Authorize(Restaurant restaurant, ResourceOperation operation)
    {
        var user = userContext.GetCurrentUser();
        logger.LogInformation("Authorizing user {UserEmail}, to {Operation} for restaurant {RestaurantName}",
            user.Email,
            operation,
            restaurant.Name);

        if (operation == ResourceOperation.Read || operation == ResourceOperation.Create)
        {
            logger.LogInformation("Create/read operator - successful authorization");
            return true;
        }

        if (operation == ResourceOperation.Delete && user.IsInRole(UserRoles.Admin))
        {
            logger.LogInformation("Administrator user,delete operation -successful authorization");
            return true;
        }

        if (operation == ResourceOperation.Delete || operation == ResourceOperation.Update
            &&user.Id==restaurant.OwnerId)
        {
            logger.LogInformation("Restaurant owner -successful authorization");
            return true;
        }

        return false;
    }
}