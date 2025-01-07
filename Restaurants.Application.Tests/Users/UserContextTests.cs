using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using Restaurants.Application.Users;
using Restaurants.Domain.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Application.Tests.Users;

public class UserContextTests
{
    // TestMethod_Scenario_ExpectResult
    [Fact()]
    public void GetCurrentUser_WithAuthenticatedUser_ShouldReturnCurrentUser()
    {
        //arrange
        var dateOfBirth = new DateOnly(2000, 1, 1);

        var httpContextAccessorMock = new Mock<IHttpContextAccessor>();


        var claims = new List<Claim>()
        {
            new(ClaimTypes.NameIdentifier,"1"),
            new(ClaimTypes.Email,"test1@test.com"),
            new(ClaimTypes.Role,UserRoles.Admin),
            new(ClaimTypes.Role,UserRoles.User),
            new("Nationality","Vietnamese"),
            new("DateOfBirth",dateOfBirth.ToString("yyyy-MM-dd")),
        };

        var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "Test"));

        httpContextAccessorMock.Setup(x => x.HttpContext).Returns(new DefaultHttpContext() { User = user });
        var userContext = new UserContext(httpContextAccessorMock.Object);

        //act
        var currentUser = userContext.GetCurrentUser();

        //asset

        currentUser.Should().NotBeNull();
        currentUser!.Id.Should().Be("1");
        currentUser.Email.Should().Be("test1@test.com");
        currentUser.Roles.Should().ContainInOrder(UserRoles.Admin, UserRoles.User);
        currentUser.Nationality.Should().Be("Vietnamese");
        currentUser.DateOfBirth.Should().Be(dateOfBirth);

    }

    [Fact()]
    public void GetCurrentUser_WithUserContextNotPresent_ThrowsInvalidOperationException()
    {
        //arrange

        var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        httpContextAccessorMock.Setup(x => x.HttpContext).Returns((HttpContext)null);
        var userContext = new UserContext(httpContextAccessorMock.Object);

        //act
        Action action = () => userContext.GetCurrentUser();

        //asset
        action.Should().Throw<InvalidOperationException>()
            .WithMessage("User context is not present");
    }
}
