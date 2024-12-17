using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Restaurants.DTOs;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Queries.GetAllRestaurants;

public class GetAllRestaurantsQueryHandle(IRestaurantsRepository restaurantsRepository,
        ILogger<GetAllRestaurantsQueryHandle> logger,
        IMapper mapper) : IRequestHandler<GetAllRestaurantsQuery, IEnumerable<RestaurantDTO>>
{
    public async Task<IEnumerable<RestaurantDTO>> Handle(GetAllRestaurantsQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting all restaurants");
        var restaurants = await restaurantsRepository.GetAllAsync();
        var restaurantsDTOs = mapper.Map<IEnumerable<RestaurantDTO>>(restaurants);
        return restaurantsDTOs;
    }
}