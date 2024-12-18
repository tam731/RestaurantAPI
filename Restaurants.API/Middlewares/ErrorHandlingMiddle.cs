﻿using Restaurants.Domain.Exceptions;

namespace Restaurants.API.Middlewares
{
    public class ErrorHandlingMiddle(ILogger<ErrorHandlingMiddle> logger) : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
			try
			{
				await next.Invoke(context);
			}
			catch (NotFoundException notFound)
			{
                context.Response.StatusCode = 404;
                await context.Response.WriteAsync(notFound.Message);
				logger.LogWarning(notFound.Message);
            }
			catch (Exception ex)
			{
				logger.LogError(ex, ex.Message);
				context.Response.StatusCode = 500;
				await context.Response.WriteAsync("Some thing went wrong");
			}
        }
    }
}
