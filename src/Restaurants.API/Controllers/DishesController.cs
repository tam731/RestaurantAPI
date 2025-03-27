using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurants.API.Attributes;
using Restaurants.Application.Dishes.Commands.CreateDish;
using Restaurants.Application.Dishes.Commands.DeleteDishesForRestaurant;
using Restaurants.Application.Dishes.DTOs;
using Restaurants.Application.Dishes.Queries.GetDishByIdForRestaurant;
using Restaurants.Application.Dishes.Queries.GetDishesForRestaurant;
using Restaurants.Domain.Interfaces;
using Restaurants.Infrastructure.Authorization;

namespace Restaurants.API.Controllers;

[ApiController]
[Authorize]
[Route("api/restaurants/{restaurantId}/dishes")]
public class DishesController(IMediator mediator, IResponseCacheService responseCacheService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateDish([FromRoute] int restaurantId, CreateDishCommand command)
    {
        command.RestaurantId = restaurantId;
        var dishId = await mediator.Send(command);
        await responseCacheService.RemoveCacheResponseAsync($"/api/restaurants/{restaurantId}/dishes");
        return CreatedAtAction(nameof(GetByIdForRestaurant), new { restaurantId , dishId },null);
    }

    [HttpGet]
    [Authorize(Policy =PolicyNames.AtLeast20)]
    [Cache(1000)]
    public async Task<ActionResult<IEnumerable<DishDTO>>> GetAllForRestaurant([FromRoute] int restaurantId)
    {
        var dishes = await mediator.Send(new GetDishesForRestaurantQuery(restaurantId));
        return Ok(dishes);
    }

    [HttpGet("{dishId}")]
    [Cache(1000)]
    public async Task<ActionResult<IEnumerable<DishDTO>>> GetByIdForRestaurant([FromRoute] int restaurantId, [FromRoute] int dishId)
    {
        var dishes = await mediator.Send(new GetDishByIdForRestaurantQuery(restaurantId, dishId));
        return Ok(dishes);
    }

    [HttpDelete("{dishId}")]
    public async Task<ActionResult<IEnumerable<DishDTO>>> DeleteDishesForRestaurant([FromRoute] int restaurantId)
    {
        await mediator.Send(new DeleteDishesForRestaurantCommand(restaurantId));
        await responseCacheService.RemoveCacheResponseAsync($"/api/restaurants/{restaurantId}/dishes");
        return NoContent();
    }
}