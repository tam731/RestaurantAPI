using Restaurants.API.Extensions;
using Restaurants.API.MiddleWares;
using Restaurants.Application.Extensions;
using Restaurants.Domain.Entities.Identity;
using Restaurants.Infrastructure.Extensions;
using Restaurants.Infrastructure.Seeders;
using Serilog;

try
{
	var builder = WebApplication.CreateBuilder(args);

	// Add services to the container.

	builder.AddPresentation();
	builder.Services.AddApplication();
	builder.Services.AddInfrastructure(builder.Configuration);

	var app = builder.Build();

	var scope = app.Services.CreateScope();
	var seeder = scope.ServiceProvider.GetRequiredService<IRestaurantSeeder>();
	await seeder.Seed();

	// Configure the HTTP request pipeline.
	app.UseMiddleware<ErrorHandlingMiddle>();
	app.UseMiddleware<RequestTimeLoggingMiddleware>();

	app.UseSerilogRequestLogging();
	if (app.Environment.IsDevelopment())
	{
		app.UseSwagger();
		app.UseSwaggerUI();
	}

	app.UseHttpsRedirection();

	app.MapGroup("api/identity").WithTags("Identity").MapIdentityApi<User>();

	app.UseAuthorization();

	app.MapControllers();

	app.Run();
}
catch (Exception ex)
{
	Log.Fatal(ex, "Application startup failed");
}
finally
{
	Log.CloseAndFlush();
}

public partial class Program() { }