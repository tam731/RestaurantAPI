using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Application.Restaurants.Commands.UploadRestaurantLogo;

internal class UploadRestaurantLogoCommandHandler(ILogger<UploadRestaurantLogoCommandHandler> logger,
    IRestaurantsRepository restaurantsRepository,
    IRestaurantsAuthorizationService restaurantsAuthorizationService,
    IBlobStorageService blobStorageService
    ) : IRequestHandler<UploadRestaurantLogoCommand>
{
    public async Task Handle(UploadRestaurantLogoCommand request, CancellationToken cancellationToken)
    {

        try
        {
            logger.LogInformation("Uploading restaurant logo for id: {RestaurantId}", request.RestaurantId);
            var restaurant = await restaurantsRepository.GetByIdAsync(request.RestaurantId)
                ?? throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());

            if (!restaurantsAuthorizationService.Authorize(restaurant, Domain.Constants.ResourceOperation.Update))
                throw new ForbiddenException();

            var logoUrl = await blobStorageService.UploadToBlobAsync(request.File, request.FileName);

            restaurant.LogoUrl = logoUrl;

            await restaurantsRepository.SaveChangeAsync();
        }
        catch (Exception e)
        {

            throw new BadRequestException($"Uploading restaurant logo occurring an error: {e.Message}");
        }
    }
}
