using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Entities.Identity;
using Restaurants.Infrastructure.Persistence;

namespace Restaurants.Infrastructure.Seeders
{
    internal class RestaurantSeeder(UserManager<User> userManager, RestaurantsDbContext dbContext) : IRestaurantSeeder
    {
        public async Task Seed()
        {
            if (dbContext.Database.GetPendingMigrations().Any())
            {
                await dbContext.Database.MigrateAsync();
            }

            if (await dbContext.Database.CanConnectAsync())
            {
                if (!dbContext.Users.Any())
                {
                    var users = GetUsers();
                    foreach (var user in users)
                    {
                        await userManager.CreateAsync(user, "User123@");
                        switch (user.Email)
                        {
                            case "admin@test.com":
                                await userManager.AddToRoleAsync(user, UserRoles.Admin);
                                break;

                            case "owner@test.com":
                                await userManager.AddToRoleAsync(user, UserRoles.Owner);
                                break;

                            case "user@test.com":
                                await userManager.AddToRoleAsync(user, UserRoles.User);
                                break;
                        }
                    }
                }
                if (!dbContext.Restaurants.Any())
                {
                    var owner = dbContext.Users.Where(u => u.Email == "owner@test.com").FirstOrDefault();
                    if (owner is not null)
                    {
                        var restaurants = GetRestaurants(owner.Id.ToString());
                        dbContext.Restaurants.AddRange(restaurants);
                        await dbContext.SaveChangesAsync();
                    }
                }

                if (!dbContext.Roles.Any())
                {
                    dbContext.Roles.AddRange(GetRoles());
                    await dbContext.SaveChangesAsync();
                }
            }
        }

        private IEnumerable<IdentityRole> GetRoles()
        {
            List<IdentityRole> roles =
                [
                   new (UserRoles.User){
                       NormalizedName=UserRoles.User.ToUpper()
                   },
                   new (UserRoles.Owner){
                       NormalizedName=UserRoles.Owner.ToUpper()
                   },
                   new (UserRoles.Admin){
                       NormalizedName=UserRoles.Admin.ToUpper()
                   }
                ];
            return roles;
        }

        private IEnumerable<User> GetUsers()
        {
            IEnumerable<User> users = [
               new (){
                    UserName="admin@test.com",
                    NormalizedEmail="admin@test.com".ToUpper(),
                    Email="admin@test.com",
                    Nationality="Vietnamese"
                },
                new (){
                     UserName="owner@test.com",
                    NormalizedEmail="owner@test.com".ToUpper(),
                    Email="owner@test.com",
                    Nationality="Vietnamese"
                },
                new (){
                     UserName="user@test.com",
                    NormalizedEmail="user@test.com".ToUpper(),
                    Email="user@test.com",
                    Nationality="Vietnamese"
                }
               ];
            return users;
        }

        private IEnumerable<Restaurant> GetRestaurants(string ownerId)
        {
            List<Restaurant> restaurants = [
                new()
                {
                    OwnerId = ownerId,
                    Name = "KFC",
                    Category = "Fast Food",
                    Description =
                        "KFC (short for Kentucky Fried Chicken) is an American fast food restaurant chain headquartered in Louisville, Kentucky, that specializes in fried chicken.",
                    ContactEmail = "contact@kfc.com",
                    HasDelivery = true,
                    Dishes =
                    [
                        new()
                        {
                            Name = "Nashville Hot Chicken",
                            Description = "Nashville Hot Chicken (10 pcs.)",
                            Price = 10.30M,
                        },

                        new()
                        {
                            Name = "Chicken Nuggets",
                            Description = "Chicken Nuggets (5 pcs.)",
                            Price = 5.30M,
                        },
                    ],
                    Address = new()
                    {
                        City = "London",
                        Street = "Cork St 5",
                        PostalCode = "WC2N 5DU"
                    }
                },
                new()
                {
                    OwnerId = ownerId,
                    Name = "McDonald",
                    Category = "Fast Food",
                    Description =
                        "McDonald's Corporation (McDonald's), incorporated on December 21, 1964, operates and franchises McDonald's restaurants.",
                    ContactEmail = "contact@mcdonald.com",
                    HasDelivery = true,
                    Address = new Address()
                    {
                        City = "London",
                        Street = "Boots 193",
                        PostalCode = "W1F 8SR"
                    }
                }
            ];

            return restaurants;
        }
    }
}