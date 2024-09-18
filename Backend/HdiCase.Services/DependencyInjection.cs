using Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static void Inject(IServiceCollection service)
    {
        service.AddHttpContextAccessor();
        service.AddSingleton<IClaimService, ClaimService>();
        service.AddSingleton<IRabbitMQService, RabbitMQService>();
        service.AddSingleton(typeof(IDatabaseContext<>), typeof(DatabaseContext<>));
        service.AddSingleton(typeof(IPublisherService<>), typeof(PublisherService<>));
        service.AddSingleton<IJWTTokenService, JWTTokenService>();
        service.AddSingleton<IRedisCacheService, RedisCacheService>();

        #region services
        service.AddScoped<IAuthenticationService, AuthenticationService>();
        #endregion
    }
}
