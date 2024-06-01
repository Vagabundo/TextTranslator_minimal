using Microsoft.Extensions.Caching.Distributed;
using MinimalTranslator.Application.Abstractions.Data;
using Newtonsoft.Json;
using System.Collections.Concurrent;

namespace MinimalTranslator.Database.Cache;

public class RedisCacheService : ICacheService
{
    private readonly static ConcurrentDictionary<string, bool> CacheKeys = new();
    private readonly IDistributedCache _cacheDb;

    public RedisCacheService (IDistributedCache cacheDb)
    {
        _cacheDb = cacheDb;
    }

    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default) where T : class
    {
        string? cachedValue = await _cacheDb.GetStringAsync(key, cancellationToken);

        if(cachedValue is null) return null;

        T? value = JsonConvert.DeserializeObject<T>(cachedValue);

        return value;
    }

    public async Task SetAsync<T>(string key, T value, CancellationToken cancellationToken = default) where T : class
    {
        string cacheValue = JsonConvert.SerializeObject(value);
        await _cacheDb.SetStringAsync(key, cacheValue, cancellationToken);

        CacheKeys.TryAdd(key, true);
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        await _cacheDb.RemoveAsync(key, cancellationToken);
        CacheKeys.TryRemove(key, out bool _);
    }

    public async Task RemoveByPrefixAsync(string prefixKey, CancellationToken cancellationToken = default)
    {
        // //slow version
        // foreach (string key in CacheKeys.Keys)
        // {
        //     if (key.StartsWith(prefixKey))
        //     {
        //         await _cacheDb.RemoveAsync(key);
        //     }
        // }

        var tasks = CacheKeys.Keys
            .Where(k => k.StartsWith(prefixKey))
            .Select(k => RemoveAsync(k, cancellationToken));

        await Task.WhenAll(tasks); 
    }

    public async Task<T> GetAsync<T>(string key, Func<Task<T>> factory, CancellationToken cancellationToken = default) where T : class
    {
        T? cachedValue = await GetAsync<T>(key, cancellationToken);

        if (cachedValue is not null) return cachedValue;

        cachedValue = await factory();
        await SetAsync(key, cachedValue, cancellationToken);

        return cachedValue;
    }
}