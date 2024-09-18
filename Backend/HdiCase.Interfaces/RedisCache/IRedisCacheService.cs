using Microsoft.Extensions.Caching.Distributed;

public interface IRedisCacheService
{
    Task<T?> GetAsync<T>(Enum_RedisCacheKeys key, params string[] extraKeys)
        where T : class;
    Task SetAsync(Enum_RedisCacheKeys key, object value, DistributedCacheEntryOptions options, params string[] extraKeys);
    Task<(T?, K?)> GetMultiAsync<T, K>(Enum_RedisCacheKeys key, params string[] extraKeys)
        where T : class;
    Task SetMultiAsync(Enum_RedisCacheKeys key, object value, object value2, DistributedCacheEntryOptions options, params string[] extraKeys);
    Task RemoveAsync(Enum_RedisCacheKeys key, params string[] extraKeys);
    Task<T?> Run<T>(Enum_RedisCacheKeys jobKey, Func<Task<T?>> action, DistributedCacheEntryOptions options, params string[] extraKeys)
        where T : class;
    List<string> GetKeys(string pattern);
    Task<bool> RemoveKeys(List<string> keys);
    Task<bool> FlushAll();
}