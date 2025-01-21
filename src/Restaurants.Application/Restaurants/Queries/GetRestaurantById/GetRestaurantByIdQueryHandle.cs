using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Restaurants.DTOs;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Queries.GetRestaurantById
{
    public class GetRestaurantByIdQueryHandle(IRestaurantsRepository restaurantsRepository,
        IBlobStorageService blobStorageService,
        ILogger<GetRestaurantByIdQueryHandle> logger,
        IMapper mapper) : IRequestHandler<GetRestaurantByIdQuery, RestaurantDTO>
    {
        public async Task<RestaurantDTO?> Handle(GetRestaurantByIdQuery request, CancellationToken cancellationToken)
        {
            var restaurant = await restaurantsRepository.GetByIdAsync(request.Id)
                           ?? throw new NotFoundException(nameof(Restaurant), request.Id.ToString()); ;
            var restaurantDTO = mapper.Map<RestaurantDTO?>(restaurant);
            restaurantDTO.LogoSasUrl = blobStorageService.GetBlobSasUrl(restaurant.LogoUrl);
            return restaurantDTO;
        }
    }
}
