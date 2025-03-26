using Microsoft.AspNetCore.Mvc.Filters;

namespace Restaurants.Infrastructure.Attributes;

internal class CacheAttribute : Attribute, IAsyncActionFilter
{
    private readonly int _cacheDurationSeconds;

    public CacheAttribute(int cacheDurationSeconds)
    {
        _cacheDurationSeconds = cacheDurationSeconds;
    }

    public Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        throw new NotImplementedException();
    }
}