using FluentValidation;
using Restaurants.Application.Restaurants.DTOs;

namespace Restaurants.Application.Restaurants.Queries.GetAllRestaurants;

public class GetAllRestaurantsQueryValidator : AbstractValidator<GetAllRestaurantsQuery>
{
    private int[] _allowPageSizes = [5, 10, 15,20, 30];
    private string[] _allowedSortByColumnNames = [nameof(RestaurantDTO.Name),
        nameof(RestaurantDTO.Category),
            nameof(RestaurantDTO.Description)];
    public GetAllRestaurantsQueryValidator()
    {
        RuleFor(r => r.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(r => r.PageSize)
            .Must(value => _allowPageSizes.Contains(value))
            .WithMessage($"Page size must be in [{string.Join(",", _allowPageSizes)}]");

        RuleFor(r => r.SortBy)
          .Must(value => _allowedSortByColumnNames.Contains(value))
          .When(q=>q.SortBy!=null)
          .WithMessage($"Sort by is optional, or must be in [{string.Join(",", _allowedSortByColumnNames)}]");

    }
}