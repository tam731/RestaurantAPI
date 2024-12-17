// Ignore Spelling: Validator


using FluentValidation;
using Restaurants.Application.Restaurants.DTOs;

namespace Restaurants.Application.Restaurants.Validators;

public class CreateRestaurantDTOValidator:AbstractValidator<CreateRestaurantDTO>
{
    private readonly List<string> validCategories = ["Italian","Mexican","Japanese","American","Indian"];
    public CreateRestaurantDTOValidator()
    {
        RuleFor(dto => dto.Name).Length(3, 100);

        RuleFor(dto => dto.Description).NotEmpty().WithMessage("Description is required.");

        RuleFor(dto => dto.Category).Must(validCategories.Contains).WithMessage("Insert a valid category. Please choose from the valid categories");

        RuleFor(dto => dto.ContactEmail).EmailAddress().WithMessage("Please provide a valid email address");

        RuleFor(dto => dto.PostalCode).Matches(@"^\d{2}-\d{3}$").WithMessage("Please provide a valid postal code (XX-XXX)");
    }
}