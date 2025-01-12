using AutoMapper;
using Castle.Core.Logging;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Application.Users;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Tests.Restaurants.Commands.CreateRestaurant;

public class CreateRestaurantCommandHandlerTests
{
    [Fact]
    public async Task Handle_ForValidCommand_ReturnsCreatedRestaurantId()
    {
        //arrange
        var loggerMock=new Mock<ILogger<CreateRestaurantCommandHandler>>();
        var mapperMock = new Mock<IMapper>();
        var command = new CreateRestaurantCommand();
        var restaurant = new Restaurant();

        mapperMock.Setup(m=>m.Map<Restaurant>(command)).Returns(restaurant);

        var restaurantRepositoryMock=new Mock<IRestaurantsRepository>();
        restaurantRepositoryMock
            .Setup(repo =>repo.CreateAsync(It.IsAny<Restaurant>()))
            .ReturnsAsync(1);

        var restaurantAuthorizeServiceMock = new Mock<IRestaurantsAuthorizationService>();
        restaurantAuthorizeServiceMock.Setup(serv => serv.Authorize(restaurant,ResourceOperation.Create))
                                      .Returns(true);

        var userContextMock = new Mock<IUserContext>();
        var currentUser= new CurrentUser("owner-id", "test1@test.com", [],null,null);
        userContextMock.Setup(u => u.GetCurrentUser())
            .Returns(currentUser);


        var commandHandler = new CreateRestaurantCommandHandler(restaurantRepositoryMock.Object,
            loggerMock.Object,
            mapperMock.Object,
            restaurantAuthorizeServiceMock.Object,
            userContextMock.Object
            );

        //act
        var result=await commandHandler.Handle(command,CancellationToken.None);

        //assert
        result.Should().Be(1);
        restaurant.OwnerId.Should().Be("owner-id");
        restaurantRepositoryMock.Verify(r=>r.CreateAsync(restaurant),Times.Once);
    }


}