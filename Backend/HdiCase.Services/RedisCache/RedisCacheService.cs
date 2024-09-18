using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;

public class RedisCacheService : IRedisCacheService
{
    private readonly IDistributedCache _distributedCache;
    public RedisCacheService(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
    }

    private string GetKey(Enum_RedisCacheKeys key, params string[] extraKeys)
    {
        try
        {
            var keyString = key.ToString();
            if (extraKeys.Length > 0)
            {
                keyString += string.Join(".", extraKeys);
            }
            return keyString;
        }
        catch (System.Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
            return "KeyGenerateErrorDumyCacheKey";
        }
    }
    public async Task<T?> GetAsync<T>(Enum_RedisCacheKeys key, params string[] extraKeys)
        where T : class
    {
        var keyString = GetKey(key, extraKeys);
        try
        {
            var value = await GetString(keyString);
            if (value == null || value == "")
            {
                return null;
            }
            return JsonSerializer.Deserialize<T>(value);
        }
        catch (System.Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
            return null;
        }
    }

    public async Task RemoveAsync(Enum_RedisCacheKeys key, params string[] extraKeys)
    {
        var keyString = GetKey(key, extraKeys);
        try
        {
            await _distributedCache.RemoveAsync(keyString);
        }
        catch (System.Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }

    public async Task SetAsync(Enum_RedisCacheKeys key, object value, DistributedCacheEntryOptions options, params string[] extraKeys)
    {
        var keyString = GetKey(key, extraKeys);
        try
        {
            await SetString(keyString, value, options);
        }
        catch (System.Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }

    public async Task<(T?, K?)> GetMultiAsync<T, K>(Enum_RedisCacheKeys key, params string[] extraKeys)
        where T : class
    {
        var keyString = GetKey(key, extraKeys);
        try
        {
            var value = await GetString(keyString);
            if (value == null)
            {
                return default;
            }
            var value2 = await GetString($"{keyString}.second");
            if (value2 == null)
            {
                value2 = "";
            }
            return (JsonSerializer.Deserialize<T>(value), JsonSerializer.Deserialize<K>(value2));
        }
        catch (System.Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
            return default;
        }
    }

    public async Task SetMultiAsync(Enum_RedisCacheKeys key, object value, object value2, DistributedCacheEntryOptions options, params string[] extraKeys)
    {
        var keyString = GetKey(key, extraKeys);
        try
        {
            await SetString(keyString, value, options);

            await SetString($"{keyString}.second", value2, options);
        }
        catch (System.Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }

    public async Task<bool> FlushAll()
    {
        try
        {
            var redisHost = EnvironmentSettings.RedisHost;
            var redisPort = EnvironmentSettings.RedisPort;
            var redisPassword = EnvironmentSettings.RedisPassword;
            var connection = ConnectionMultiplexer.Connect($"{redisHost}:{redisPort},password={redisPassword},allowAdmin=true");
            var flushedKeys = new List<string>();
            foreach (var endPoint in connection.GetEndPoints())
            {
                var server = connection.GetServer(endPoint);
                var keys = server.Keys(0);
                await server.FlushDatabaseAsync(0);
            }
        }
        catch
        {
            return false;
        }
        return true;
    }

    public async Task<T?> Run<T>(Enum_RedisCacheKeys jobKey, Func<Task<T?>> action, DistributedCacheEntryOptions options, params string[] extraKeys)
        where T : class
    {
        var result = await GetAsync<T>(jobKey, extraKeys);
        var type = typeof(T);
        if (
            result is null ||
            (result is string && Convert.ToString(result)?.Length > 0) ||
            (result is List<object> && type.IsGenericType && type.GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>)))
        )
        {
            try
            {
                result = await action();
                if (result != null)
                {
                    await SetAsync(jobKey, result, options);
                }
            }
            catch
            {
                await SetAsync(jobKey, "", options);
            }
        }
        return result;
    }

    public List<string> GetKeys(string pattern)
    {
        var redisHost = EnvironmentSettings.RedisHost;
        var redisPort = EnvironmentSettings.RedisPort;
        var redisPassword = EnvironmentSettings.RedisPassword;
        var connection = ConnectionMultiplexer.Connect($"{redisHost}:{redisPort},password={redisPassword},allowAdmin=true");
        var flushedKeys = new List<string>();
        foreach (var endPoint in connection.GetEndPoints())
        {
            var server = connection.GetServer(endPoint);
            var keys = server.Keys(0, pattern);
            if (keys != null && keys.Count() > 350)
            {
                keys = keys.Take(350);
            }
            if (keys != null)
                flushedKeys.AddRange(keys.Select(x => x.ToString()));
            if (flushedKeys.Count > 349)
            {
                continue;
            }
        }
        return flushedKeys;
    }

    public async Task<bool> RemoveKeys(List<string> keys)
    {
        try
        {
            var redisHost = EnvironmentSettings.RedisHost;
            var redisPort = EnvironmentSettings.RedisPort;
            var redisPassword = EnvironmentSettings.RedisPassword;
            var connection = ConnectionMultiplexer.Connect($"{redisHost}:{redisPort},password={redisPassword},allowAdmin=true");
            var _db = connection.GetDatabase();
            foreach (var endPoint in connection.GetEndPoints())
            {
                var server = connection.GetServer(endPoint);
                foreach (var key in keys)
                {
                    await _db.KeyDeleteAsync(key);
                }
            }
        }
        catch
        {
            return false;
        }
        return true;
    }

    private async Task<string?> GetString(string jobKey)
    {
        var hashValue = await _distributedCache.GetStringAsync(jobKey);
        if (hashValue is not null && hashValue != "")
        {
            var bytesValue = JsonSerializer.Deserialize<List<byte>>(hashValue);
            if (bytesValue != null && bytesValue.Count > 0)
            {
                var value = Encoding.UTF8.GetString(bytesValue.ToArray());
                return value;
            }
        }
        return null;
    }

    private async Task SetString(string jobKey, object value, DistributedCacheEntryOptions options)
    {
        var valueString = JsonSerializer.Serialize(value);
        var byteValue = Encoding.UTF8.GetBytes(valueString);
        var byteString = JsonSerializer.Serialize(byteValue.ToList());
        await _distributedCache.SetStringAsync(jobKey, byteString, options);
    }
}