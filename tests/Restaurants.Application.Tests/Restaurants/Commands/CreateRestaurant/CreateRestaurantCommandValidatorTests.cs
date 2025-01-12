using FluentAssertions;
using FluentValidation.TestHelper;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;

namespace Restaurants.Application.Tests.Restaurants.Commands.CreateRestaurant;

public class CreateRestaurantCommandValidatorTests
{
    // TestMethod_Scenario_ExpectResult
    [Fact]
    public void Validator_ForValidCommand_ShouldNotHaveValidationErrors()
    {
        //arrange
        var command=new CreateRestaurantCommand() 
        { 
            Name="Test",
            Category= "Vietnamese",
            ContactEmail="test1@test.com",
            PostalCode="12-345",
            Description="hello"
        };

        var validator = new CreateRestaurantCommandValidator();

        //act
        var result=validator.TestValidate(command);

        //
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validator_ForValidCommand_ShouldHaveValidationErrors()
    {
        //arrange
        var command = new CreateRestaurantCommand()
        {
            Name = "Te",
            Category = "Vi",
            ContactEmail = "@test.com",
            PostalCode = "12345",
            Description = "hello"
        };

        var validator = new CreateRestaurantCommandValidator();

        //act
        var result = validator.TestValidate(command);

        //
        result.ShouldHaveValidationErrorFor(c=>c.Name);
        result.ShouldHaveValidationErrorFor(c=>c.Category);
        result.ShouldHaveValidationErrorFor(c=>c.ContactEmail);
        result.ShouldHaveValidationErrorFor(c=>c.PostalCode);
    }

    [Theory]
    [InlineData("Vietnamese")]
    [InlineData("Mexican")]
    [InlineData("Japanese")]
    [InlineData("American")]
    [InlineData("Indian")]
    public void Validator_forValidCategory_ShouldNotHaveValidationErrorsCategoryProperty(string category)
    {
        //arrange
        var validator=new CreateRestaurantCommandValidator();
        var command = new CreateRestaurantCommand { Category = category };
        //act

        var result = validator.TestValidate(command);
        //assert
        result.ShouldNotHaveValidationErrorFor(c => c.Category);
    }

    [Theory]
    [InlineData("12345")]
    [InlineData("123-56")]
    [InlineData("12 345")]
    [InlineData("12-3 45")]
    public void Validator_ForInvalidPostalCode_ShouldHaveValidationErrorsForPostalCodeProperty(string postalCode)
    {
        //arrange
        var validator = new CreateRestaurantCommandValidator();
        var command = new CreateRestaurantCommand { PostalCode = postalCode };
        //act

        var result = validator.TestValidate(command);
        //assert
        result.ShouldHaveValidationErrorFor(c => c.PostalCode);
    }
}
