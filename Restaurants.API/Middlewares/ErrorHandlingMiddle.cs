using Restaurants.Domain.Exceptions;
using System.Text.Json;

namespace Restaurants.API.MiddleWares
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
            catch (ForbiddenException)
            {
                context.Response.StatusCode = 403;
                await context.Response.WriteAsync("Access forbidden");
            }
            catch (BadRequestException ex)
            {
                context.Response.StatusCode = 403;
                await context.Response.WriteAsync(ex.Message);
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