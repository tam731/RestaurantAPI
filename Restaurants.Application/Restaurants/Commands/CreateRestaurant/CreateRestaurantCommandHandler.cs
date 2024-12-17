using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands.CreateRestaurant;

public class CreateRestaurantCommandHandler(IRestaurantsRepository restaurantsRepository,
    ILogger<CreateRestaurantCommandHandler> logger,
    IMapper mapper) : IRequestHandler<CreateRestaurantCommand, int>
{
    public async Task<int> Handle(CreateRestaurantCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var restaurant = mapper.Map<Restaurant>(request);

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