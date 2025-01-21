using FluentAssertions;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using Restaurants.Application.Restaurants.DTOs;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using System.Net.Http.Json;

namespace Restaurants.API.Tests.Controllers;

public class RestaurantsControllerTests:IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _applicationFactory;
    private readonly Mock<IRestaurantsRepository> _restaurantsRepositoryMock=new();
    public RestaurantsControllerTests(WebApplicationFactory<Program> webApplicationFactory)
    {
        _applicationFactory = webApplicationFactory.WithWebHostBuilder(builder => 
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddSingleton<IPolicyEvaluator,FakePolicyEvaluator>();
                services.Replace(ServiceDescriptor.Scoped(typeof(IRestaurantsRepository),
                                        _=> _restaurantsRepositoryMock.Object));
            });
        }) ;
    }
   
    [Fact]
    public async Task GetAll_ForValidRequest_ShouldReturns200Ok()
    {
        //arrange
        var client=_applicationFactory.CreateClient();

        //action
        var response = await client.GetAsync("/api/restaurants?pageNumber=1&pageSize=10");

        //assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }

    //[Fact]
    //public async Task GetAll_ForInvalidRequest_ShouldReturns400BadRequest()
    //{
    //    //arrange
    //    var client = _applicationFactory.CreateClient();

    //    //action
    //    var response = await client.GetAsync("/api/restaurants");

    //    //assert
    //    response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    //}
    [Fact]
    public async Task GetById_ForNonExistingId_ShouldReturns404NotFound()
    {
        //arrange
        var id = 1234;

        _restaurantsRepositoryMock.Setup(m => m.GetByIdAsync(id)).ReturnsAsync((Restaurant?)null);

        var client = _applicationFactory.CreateClient();

        //action
        var response = await client.GetAsync($"/api/restaurants/{id}");

        //assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }
    [Fact]
    public async Task GetById_ForExistingId_ShouldReturns200Ok()
    {
        //arrange
        var id = 5;
        var restaurant=new Restaurant { Id = id ,Name="Test",Description="test description"};
        _restaurantsRepositoryMock.Setup(m => m.GetByIdAsync(id)).ReturnsAsync(restaurant);

        var client = _applicationFactory.CreateClient();

        //action
        var response = await client.GetAsync($"/api/restaurants/{id}");
        var restaurantDto=await response.Content.ReadFromJsonAsync<RestaurantDTO>();

        //assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        restaurantDto.Should().NotBeNull();
        restaurantDto.Name.Should().Be("Test");
        restaurantDto.Description.Should().Be("test description");

    }
}
