using FluentAssertions;
using Restaurants.Application.Users;
using Restaurants.Domain.Constants;

namespace Restaurants.Application.Tests;

public class CurrentUserTests
{
    // TestMethod_Scenario_ExpectResult
    [Theory]
    [InlineData(UserRoles.Admin)]
    [InlineData(UserRoles.User)]
    public void IsInRole_WithMatchingRole_ShouldReturnTrue(string roleName)
    {
        //arrange
        var currentUser = new CurrentUser("1", "test1@test.com", [UserRoles.Admin, UserRoles.User], null, null);

        //act
        bool isInRole= currentUser.IsInRole(roleName);

        //assert
        isInRole.Should().BeTrue();
    }

    [Fact]
    public void IsInRole_WithMatchingRole_ShouldReturnFalse()
    {
        //arrange
        var currentUser = new CurrentUser("1", "test1@test.com", [UserRoles.Admin, UserRoles.User], null, null);

        //act
        bool isInRole = currentUser.IsInRole(UserRoles.Admin.ToLower());

        //assert
        isInRole.Should().BeFalse();
    }
}