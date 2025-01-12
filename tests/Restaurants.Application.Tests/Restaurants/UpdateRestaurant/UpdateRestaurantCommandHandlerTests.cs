using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Application.Restaurants.Commands.UpdateRestaurant;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Tests.Restaurants.UpdateRestaurant;

public class UpdateRestaurantCommandHandlerTests
{
    private readonly Mock<ILogger<UpdateRestaurantCommandHandler>> _loggerMock;
    private readonly Mock<IRestaurantsRepository> _restaurantsRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IRestaurantsAuthorizationService> _restaurantsAuthorizationServiceMock;
    private readonly UpdateRestaurantCommandHandler _handler;

    public UpdateRestaurantCommandHandlerTests()
    {
        _loggerMock = new Mock<ILogger<UpdateRestaurantCommandHandler>>();
        _restaurantsRepositoryMock = new Mock<IRestaurantsRepository>();
        _mapperMock = new Mock<IMapper>();
        _restaurantsAuthorizationServiceMock = new Mock<IRestaurantsAuthorizationService>();

        _handler = new UpdateRestaurantCommandHandler(
            _loggerMock.Object,
            _restaurantsRepositoryMock.Object,
            _mapperMock.Object,
            _restaurantsAuthorizationServiceMock.Object);
    }

    [Fact]
    public async void Handle_WithValidRequest_ShouldUpdateRestaurants()
    {
        //arrange
        var restaurantId = 1;
        var command = new UpdateRestaurantCommand()
        {
            Id = restaurantId,
            Name = "New Test",
            Description="New Description",
            HasDelivery=true,
        };

        var restaurant = new Restaurant()
        {
            Id = restaurantId,
            Name = "Test",
            Description = "Test",
        };

        _restaurantsRepositoryMock.Setup(r => r.GetByIdAsync(restaurantId))
            .ReturnsAsync(restaurant);

        _restaurantsAuthorizationServiceMock.Setup(m => m.Authorize(restaurant, Domain.Constants.ResourceOperation.Update))
                                            .Returns(true);

        

        //act
        await _handler.Handle(command,CancellationToken.None);

        //assert
        _restaurantsRepositoryMock.Verify(r=>r.SaveChangeAsync(),Times.Once());
        _mapperMock.Verify(m=>m.Map(command,restaurant), Times.Once());
    }

    [Fact]
    public async void Handle_NotFoundRestaurant_ShouldThrowNotFoundException()
    {
        //Arrange
        var restaurantId = 2;

        var request = new UpdateRestaurantCommand { Id = restaurantId };
        _restaurantsRepositoryMock.Setup(r => r.GetByIdAsync(restaurantId))
                                  .ReturnsAsync((Restaurant)null);
        // act

        Func<Task> act= async () => await _handler.Handle(request,CancellationToken.None);

        //assert
        await act.Should().ThrowAsync<NotFoundException>()
                          .WithMessage($"Restaurant with id: {restaurantId} does not exist");
    }

    [Fact]
    public async void Handle_WithUnauthorizedUser_ShouldThrowForbiddenException()
    {
        //Arrange
        var restaurantId = 3;

        var request = new UpdateRestaurantCommand { Id = restaurantId };
        var existRestaurant = new Restaurant { Id = restaurantId };
        _restaurantsRepositoryMock.Setup(r => r.GetByIdAsync(restaurantId))
                                  .ReturnsAsync(existRestaurant);

        _restaurantsAuthorizationServiceMock.Setup(m => m.Authorize(existRestaurant, Domain.Constants.ResourceOperation.Update))
                                            .Returns(false);
        // act

        Func<Task> act = async () => await _handler.Handle(request, CancellationToken.None);

        //assert
        await act.Should().ThrowAsync<ForbiddenException>();
    }
}