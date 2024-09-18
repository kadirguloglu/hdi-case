using Microsoft.Extensions.DependencyInjection;

public static class InitRedis
{
    public static void Init(IServiceCollection service)
    {
        service.AddStackExchangeRedisCache(options =>
        {
            var redisHost = EnvironmentSettings.RedisHost;
            var redisPort = EnvironmentSettings.RedisPort;
            var redisPassword = EnvironmentSettings.RedisPassword;
            options.ConfigurationOptions = new StackExchange.Redis.ConfigurationOptions()
            {
                Password = redisPassword,
                EndPoints = {
                    $"{redisHost}:{redisPort}"
                }
            };
        });
    }
}