using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Restaurants.Domain.Interfaces;
using Restaurants.Infrastructure.Configuration;
using System.Text;

namespace Restaurants.API.Attributes;

public class CacheAttribute : Attribute, IAsyncActionFilter
{
    private readonly int _cacheDurationSeconds;

    public CacheAttribute(int cacheDurationSeconds=1000)
    {
        _cacheDurationSeconds = cacheDurationSeconds;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var cacheConfiguration=context.HttpContext.RequestServices.GetRequiredService<RedisConfiguration>();

        if (!cacheConfiguration.Enabled) 
        { 
            await next();
            return;
        }
        var cacheService=context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();

        var cacheKey =GenerateCacheKeyFromRequest(context.HttpContext.Request);
        var cacheResponse=await cacheService.GetCachedResponseAsync(cacheKey);

        if (!string.IsNullOrWhiteSpace(cacheResponse))
        {
            var contentResult = new ContentResult
            {
                Content = cacheResponse,
                ContentType = "application/json",
                StatusCode = 200
            };
            context.Result = contentResult;
            return;
        }
        //if the data is not available, store it in cache.
        var executedContext = await next();//go to controller
        if(executedContext.Result is OkObjectResult objectResult)
            await cacheService.SetCacheResponseAsync(cacheKey,objectResult.Value,TimeSpan.FromSeconds(_cacheDurationSeconds));

    }

    private static string GenerateCacheKeyFromRequest(HttpRequest request) 
    { 
        var keyBuilder=new StringBuilder();
        keyBuilder.Append($"{request.Path}");
        foreach(var(key,value) in request.Query.OrderBy(x => x.Key))
        {
            keyBuilder.Append($"|{key}-{value}");
        }
        return keyBuilder.ToString();
    }
}