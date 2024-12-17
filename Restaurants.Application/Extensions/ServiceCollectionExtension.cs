using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Restaurants.Application.Restaurants;

namespace Restaurants.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApplication(this IServiceCollection services)
        {
            var applicationAssembly = typeof(ServiceCollectionExtensions).Assembly;

            //customize body response
            //services.AddControllers()
            //        .ConfigureApiBehaviorOptions(options =>
            //        {
            //            options.InvalidModelStateResponseFactory = context =>
            //            {
            //                var errors = context.ModelState
            //                    .Where(e => e.Value.Errors.Count > 0)
            //                    .ToDictionary(
            //                        e => e.Key,
            //                        e => e.Value.Errors.Select(x => x.ErrorMessage).ToArray()
            //                    );

            //                return new BadRequestObjectResult(new { Message = "Validation Failed", Errors = errors });
            //            };
            //        });

            //MediatR
            services.AddMediatR(cfg =>  cfg.RegisterServicesFromAssembly(applicationAssembly));

            //Service
            services.AddScoped<IRestaurantsService, RestaurantsService>();

            //auto-mapper
            services.AddAutoMapper(applicationAssembly);


            services.AddValidatorsFromAssembly(applicationAssembly)
                .AddFluentValidationAutoValidation();
        }
    }
}