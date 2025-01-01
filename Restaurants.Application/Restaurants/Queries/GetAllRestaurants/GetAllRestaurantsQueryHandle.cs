using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Common;
using Restaurants.Application.Restaurants.DTOs;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Queries.GetAllRestaurants;

public class GetAllRestaurantsQueryHandle(IRestaurantsRepository restaurantsRepository,
        ILogger<GetAllRestaurantsQueryHandle> logger,
        IMapper mapper) : IRequestHandler<GetAllRestaurantsQuery, PagedResult<RestaurantDTO>>
{
    public async Task<PagedResult<RestaurantDTO>> Handle(GetAllRestaurantsQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting all restaurants");
        var (restaurants,totalCount) = await restaurantsRepository.GetAllMatchingAsync(request.SearchPhrase,
                                                                    request.PageSize,
                                                                    request.PageNumber,
                                                                    request.SortBy,
                                                                    request.SortDirection);
        var restaurantsDTOs = mapper.Map<IEnumerable<RestaurantDTO>>(restaurants);

        var result=new PagedResult<RestaurantDTO>(restaurantsDTOs, totalCount, request.PageSize,request.PageNumber);
        return result;
    }
}