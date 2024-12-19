using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Entities.Identity;

namespace Restaurants.Infrastructure.Persistence
{
    internal class RestaurantsDbContext(DbContextOptions<RestaurantsDbContext> options) 
        : IdentityDbContext<User>(options)
    {
        internal DbSet<Restaurant> Restaurants { get; set; }
        internal DbSet<Dish> Dishes { get; set; }
        internal DbSet<Address> Addresses { get; set; }
    }
}