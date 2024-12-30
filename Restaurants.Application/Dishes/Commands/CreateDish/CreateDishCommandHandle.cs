using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Dishes.Commands.CreateDish;

public class CreateDishCommandHandle(ILogger<CreateDishCommandHandle> logger,
    IRestaurantsRepository restaurantsRepository,
    IDishesRepository dishesRepository,
    IMapper mapper,
    IRestaurantsAuthorizationService restaurantsAuthorizationService) : IRequestHandler<CreateDishCommand, int>
{
    public async Task<int> Handle(CreateDishCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating new dish: {@DishRequest}", request);
        var restaurant = await restaurantsRepository.GetByIdAsync(request.RestaurantId)
            ?? throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());
        if (!restaurantsAuthorizationService.Authorize(restaurant, ResourceOperation.Create))
            throw new ForbiddenException();
        try
        {
            var dish = mapper.Map<Dish>(request);

            return await dishesRepository.Create(dish);
        }
        catch (Exception e)
        {
            throw new BadRequestException("Error : " + e.Message);
        }
    }
}