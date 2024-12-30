using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands.DeleteRestaurant;

public class DeleteRestaurantCommandHandler(ILogger<DeleteRestaurantCommandHandler> logger,
    IRestaurantsRepository restaurantsRepository,
    IRestaurantsAuthorizationService restaurantsAuthorizationService) : IRequestHandler<DeleteRestaurantCommand>
{
    public async Task Handle(DeleteRestaurantCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Deleting restaurant with id : {request.Id}");
        var restaurant = await restaurantsRepository.GetByIdAsync(request.Id)
                        ?? throw new NotFoundException(nameof(Restaurant), request.Id.ToString());
        if (!restaurantsAuthorizationService.Authorize(restaurant, ResourceOperation.Delete))
            throw new ForbiddenException();
        try
        {
            await restaurantsRepository.DeleteAsync(restaurant);
        }
        catch (Exception e)
        {
            throw new BadRequestException("Error : " + e.Message);
        }
    }
}