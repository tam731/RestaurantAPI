using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Users;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands.CreateRestaurant;

public class CreateRestaurantCommandHandler(IRestaurantsRepository restaurantsRepository,
    ILogger<CreateRestaurantCommandHandler> logger,
    IMapper mapper,
    IRestaurantsAuthorizationService restaurantsAuthorizationService,
    IUserContext userContext) : IRequestHandler<CreateRestaurantCommand, int>
{
    public async Task<int> Handle(CreateRestaurantCommand request, CancellationToken cancellationToken)
    {
        var restaurant = mapper.Map<Restaurant>(request);
        if (!restaurantsAuthorizationService.Authorize(restaurant, ResourceOperation.Create))
            throw new ForbiddenException();

        try
        {
            var currUser=userContext.GetCurrentUser();
            restaurant.OwnerId = currUser!.Id;
            int id = await restaurantsRepository.CreateAsync(restaurant);
            return id;
        }
        catch (Exception e)
        {
            throw new BadRequestException("Error : " + e.Message);
        }
    }
}