using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Restaurants.Domain.Interfaces;
using StackExchange.Redis;
using System.Net;

namespace Restaurants.Infrastructure.RedisCache;

internal class ResponseCacheService : IResponseCacheService
{
    private readonly IDistributedCache _distributedCache;
    private readonly IConnectionMultiplexer _connectionMultiplexer;

    public ResponseCacheService(IDistributedCache distributedCache,IConnectionMultiplexer connectionMultiplexer)
    {
        _distributedCache = distributedCache;
        _connectionMultiplexer = connectionMultiplexer;
    }
    public async Task<string> GetCachedResponseAsync(string cacheKey)
    {
        var cacheResponse=await _distributedCache.GetStringAsync(cacheKey);
        return string.IsNullOrWhiteSpace(cacheResponse) ? "" : cacheResponse;
    }

    public async Task RemoveCacheResponseAsync(string pattern)
    {
        if (string.IsNullOrWhiteSpace(pattern))
            throw new ArgumentNullException("Value cannot be null or whitespace");
        
        await foreach (var key in GetKeyAsync(pattern + "*"))
        {
            await _distributedCache.RemoveAsync(key);
        }
    }

    public async Task SetCacheResponseAsync(string cacheKey, object response, TimeSpan timeout)
    {
        if (response == null)
            return;
        var serializerResponse = JsonConvert.SerializeObject(response, new JsonSerializerSettings()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
        });

        await _distributedCache.SetStringAsync(cacheKey, serializerResponse,new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow=timeout
        });
    }

    private async IAsyncEnumerable<string> GetKeyAsync(string pattern)
    {
        if(string.IsNullOrWhiteSpace(pattern))
            throw new ArgumentNullException("Value cannot be null or whitespace");
        //get all endpoints example case with multiple redis cache
        var endpoints = _connectionMultiplexer.GetEndPoints();
        foreach (var endpoint in endpoints)
        {
            var server=_connectionMultiplexer.GetServer(endpoint);
            foreach (var key in server.Keys(pattern: pattern))
            {
                //return to continue loop
                yield return key.ToString();
            }
        }
    }
}