using Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static void Inject(IServiceCollection service)
    {
        service.AddHttpContextAccessor();
        service.AddSingleton<IClaimService, ClaimService>();
        service.AddSingleton<IRabbitMQService, RabbitMQService>();
        service.AddSingleton(typeof(IPublisherService<>), typeof(PublisherService<>));
        service.AddSingleton(typeof(IStorageService<>), typeof(StorageService<>));
        service.AddSingleton<IJWTTokenService, JWTTokenService>();
        service.AddSingleton<IRedisCacheService, RedisCacheService>();
        service.AddScoped(typeof(IDatabaseContext<>), typeof(DatabaseContext<>));

        #region services
        service.AddScoped<IAuthenticationService, AuthenticationService>();
        service.AddScoped<IAggrementService, AggrementService>();
        service.AddScoped<ICompanyService, CompanyService>();
        service.AddScoped<IAdminLoginDataService, AdminLoginDataService>();
        service.AddScoped<ILoggingService, LoggingService>();
        service.AddScoped<IRoleService, RoleService>();
        service.AddScoped<INotificationHubDispatcher, NotificationHubDispatcher>();
        #endregion
    }
}
