﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Restaurants.Domain.Entities.Identity;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Authorization;
using Restaurants.Infrastructure.Authorization.Requirements;
using Restaurants.Infrastructure.Authorization.Services;
using Restaurants.Infrastructure.Persistence;
using Restaurants.Infrastructure.Repositories;
using Restaurants.Infrastructure.Seeders;

namespace Restaurants.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            string connectionstring = configuration.GetConnectionString("RestaurantsDb")!;
            services.AddDbContext<RestaurantsDbContext>(options =>
                options.UseSqlServer(connectionstring)
                        .EnableSensitiveDataLogging());
            // Add Identity services
            //services.AddIdentity<User, IdentityRole>()
            //        .AddEntityFrameworkStores<RestaurantsDbContext>()
            //        .AddDefaultTokenProviders();

            services.AddIdentityApiEndpoints<User>()
                .AddRoles<IdentityRole>()
                .AddClaimsPrincipalFactory<RestaurantsUserClaimsPrincipalFactory>()
                .AddEntityFrameworkStores<RestaurantsDbContext>();

            services.AddScoped<IRestaurantSeeder, RestaurantSeeder>();
            //Repository

            services.AddScoped<IRestaurantsRepository, RestaurantsRepository>();
            services.AddScoped<IDishesRepository, DishesRepository>();

            services.AddAuthorizationBuilder()
                .AddPolicy(PolicyNames.HasNationality, builder => builder.RequireClaim(ApplicationClaimTypes.Nationality, "Vietnamese", "US"))
                .AddPolicy(PolicyNames.AtLeast20,
                   builder => builder.AddRequirements(new MinimumAgeRequirement(20)))
              .AddPolicy(PolicyNames.CreatedAtLeast2Restaurants,
                   builder => builder.AddRequirements(new CreateMultipleRestaurantsRequirement(2)));

            services.AddScoped<IAuthorizationHandler, MinimumAgeRequirementHandler>();
            services.AddScoped<IAuthorizationHandler, CreateMultipleRestaurantsRequirementHandler>();
            services.AddScoped<IRestaurantsAuthorizationService, RestaurantsAuthorizationService>();
        }
    }
}