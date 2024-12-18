using MediatR;
using Restaurants.Application.Dishes.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Restaurants.Application.Dishes.Queries.GetDishesForRestaurant;

public class GetDishesForRestaurantQuery(int restaurantId) : IRequest<IEnumerable<DishDTO>>
{
    public int RestaurantId { get; } = restaurantId;
}