namespace Restaurants.Domain.Interfaces;

public interface IResponseCacheService
{
    Task SetCacheResponseAsync(string cacheKey,object response,TimeSpan timeout );
    Task<string> GetCachedResponseAsync(string cacheKey);

    Task RemoveCacheResponseAsync(string pattern);
}