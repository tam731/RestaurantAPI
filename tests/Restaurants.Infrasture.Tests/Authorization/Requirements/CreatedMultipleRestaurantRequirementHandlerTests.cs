﻿using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Moq;
using Restaurants.Application.Users;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Authorization.Requirements;

namespace Restaurants.Infrastructure.Tests.Authorization.Requirements;

public class CreatedMultipleRestaurantRequirementHandlerTests
{
    [Fact]
    public async void HandleRequirementAsync_UserHasCreatedMultipleRestaurants_ShouldSucceed()
    {
        //arrange
        var currentUser = new CurrentUser("1", "test1@test.com", [], null, null);
        var userContextMock=new Mock<IUserContext>();
        userContextMock.Setup(m=>m.GetCurrentUser()).Returns(currentUser);

        var restaurants = new List<Restaurant>()
        {
            new(){OwnerId=currentUser.Id},
            new(){OwnerId=currentUser.Id},
            new(){OwnerId=currentUser.Id}
        };
        var restaurantRepositoryMock=new Mock<IRestaurantsRepository>();
        restaurantRepositoryMock.Setup(m=>m.GetAllAsync())
                                .ReturnsAsync(restaurants);
        var requirement = new CreateMultipleRestaurantsRequirement(2);
        var context = new AuthorizationHandlerContext([requirement], null, null);

        var handler = new CreateMultipleRestaurantsRequirementHandler(restaurantRepositoryMock.Object,
            userContextMock.Object);

        //act
        await handler.HandleAsync(context);

        //assert
        context.HasSucceeded.Should().BeTrue();

    }

    [Fact]
    public async void HandleRequirementAsync_UserHasCreatedMultipleRestaurants_ShouldFail()
    {
        //arrange
        var currentUser = new CurrentUser("1", "test1@test.com", [], null, null);
        var userContextMock = new Mock<IUserContext>();
        userContextMock.Setup(m => m.GetCurrentUser()).Returns(currentUser);

        var restaurants = new List<Restaurant>()
        {
            new(){OwnerId=currentUser.Id},
            new(){OwnerId="2"}
        };
        var restaurantRepositoryMock = new Mock<IRestaurantsRepository>();
        restaurantRepositoryMock.Setup(m => m.GetAllAsync())
                                .ReturnsAsync(restaurants);
        var requirement = new CreateMultipleRestaurantsRequirement(2);
        var context = new AuthorizationHandlerContext([requirement], null, null);

        var handler = new CreateMultipleRestaurantsRequirementHandler(restaurantRepositoryMock.Object,
            userContextMock.Object);

        //act
        await handler.HandleAsync(context);

        //assert
        context.HasSucceeded.Should().BeFalse();

    }
}
