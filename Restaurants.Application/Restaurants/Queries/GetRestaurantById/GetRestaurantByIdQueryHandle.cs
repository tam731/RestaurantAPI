using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Restaurants.DTOs;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Queries.GetRestaurantById
{
    public class GetRestaurantByIdQueryHandle(IRestaurantsRepository restaurantsRepository,
        ILogger<GetRestaurantByIdQueryHandle> logger,
        IMapper mapper) : IRequestHandler<GetRestaurantByIdQuery, RestaurantDTO>
    {
        public async Task<RestaurantDTO?> Handle(GetRestaurantByIdQuery request, CancellationToken cancellationToken)
        {
            var restaurant = await restaurantsRepository.GetByIdAsync(request.Id);
            var restaurantDTO = mapper.Map<RestaurantDTO?>(restaurant);
            return restaurantDTO;
        }
    }
}
