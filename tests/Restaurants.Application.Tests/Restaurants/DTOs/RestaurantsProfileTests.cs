using AutoMapper;
using FluentAssertions;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Application.Restaurants.DTOs;
using Restaurants.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Application.Tests.Restaurants.DTOs;

public class RestaurantsProfileTests
{
    private readonly IMapper _mapper;
    public RestaurantsProfileTests()
    {
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<RestaurantsProfile>();
        });
        _mapper = configuration.CreateMapper();
    }
    [Fact()]
    public void CreateMap_ForRestaurantToRestaurantDTO_MapsCorrectly()
    {
        //Arrange

        var restaurant = new Restaurant()
        {
            Id = 1,
            Name = "Test restaurant",
            Description = "Test description",
            Category = "Test category",
            HasDelivery = true,
            ContactEmail = "test@example.com",
            ContactNumber = "123456789",
            Address = new Address
            {
                City = "Test city",
                Street = "Test street",
                PostalCode = "12-345"
            }
        };
        //act

        var restaurantDTO = _mapper.Map<RestaurantDTO>(restaurant);
        //assert

        restaurantDTO.Should().NotBeNull();
        restaurantDTO.Id.Should().Be(restaurant.Id);
        restaurantDTO.Name.Should().Be(restaurant.Name);
        restaurantDTO.Description.Should().Be(restaurant.Description);
        restaurantDTO.HasDelivery.Should().Be(restaurant.HasDelivery);
        restaurantDTO.Category.Should().Be(restaurant.Category);
        restaurantDTO.City.Should().Be(restaurant.Address.City);
        restaurantDTO.Street.Should().Be(restaurant.Address.Street);
        restaurantDTO.PostalCode.Should().Be(restaurant.Address.PostalCode);
    }

    [Fact()]
    public void CreateMap_ForCreateRestaurantToRestaurantDTO_MapsCorrectly()
    {
        //Arrange
        var command = new CreateRestaurantCommand()
        {
            Name = "Test restaurant",
            Description = "Test description",
            Category = "Test category",
            HasDelivery = true,
            ContactEmail = "test@example.com",
            ContactNumber = "123456789",

            City = "Test city",
            Street = "Test street",
            PostalCode = "12-345"
        };
        //act

        var restaurant = _mapper.Map<Restaurant>(command);
        //assert

        restaurant.Should().NotBeNull();
        restaurant.Name.Should().Be(command.Name);
        restaurant.Description.Should().Be(command.Description);
        restaurant.HasDelivery.Should().Be(command.HasDelivery);
        restaurant.Category.Should().Be(command.Category);
        restaurant.Address.Should().NotBeNull();
        restaurant.Address.City.Should().Be(command.City);
        restaurant.Address.Street.Should().Be(command.Street);
        restaurant.Address.PostalCode.Should().Be(command.PostalCode);
    }
}