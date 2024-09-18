using Humanizer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.OData;

public class Startup
{
    private readonly string RouteKey = Enum_RoutingKeys.Api.ToString().Underscore();
    public void ConfigureServices(IServiceCollection services)
    {
        InitDatabaseContext.Init(services);
        var redisHost = EnvironmentSettings.RedisHost;
        var redisPort = EnvironmentSettings.RedisPort;
        var redisPassword = EnvironmentSettings.RedisPassword;
        services.AddSignalR(x =>
        {
            if (EnvironmentSettings.IsDevelopment)
            {
                x.EnableDetailedErrors = true;
            }
        })
        .AddStackExchangeRedis(options =>
        {
            var redisHost = EnvironmentSettings.RedisHost;
            var redisPort = EnvironmentSettings.RedisPort;
            var redisPassword = EnvironmentSettings.RedisPassword;
            options.Configuration = new StackExchange.Redis.ConfigurationOptions()
            {
                Password = redisPassword,
                EndPoints = {
                    $"{redisHost}:{redisPort}"
                },
            };
        });
        var mvcBuilder = services.AddControllers(mvcOptions =>
        {
            mvcOptions.EnableEndpointRouting = false;
            mvcOptions.AllowEmptyInputInBodyModelBinding = true;
        });
        InitJWT.Init(services);
        InitSwagger.Init(services);
        InitRedis.Init(services);
        DependencyInjection.Inject(services);
        InitCORS.Init(services);
        InitOData.Init(mvcBuilder);
    }

    public void Configure(IApplicationBuilder app)
    {
        if (EnvironmentSettings.IsDevelopment)
        {
            app.UseSwagger(x =>
            {
                x.RouteTemplate = RouteKey + "/swagger/{documentName}/swagger.json";
            });
            app.UseSwaggerUI(c =>
            {
                c.EnableDeepLinking();
                c.SwaggerEndpoint("/" + RouteKey + "/swagger/Mobile-v1/swagger.json", "Test");
                c.SwaggerEndpoint("/" + RouteKey + "/swagger/Api-v1/swagger.json", "Api V1.0");
                c.RoutePrefix = RouteKey + "/swagger";
            });
        }

        app.UseCors();
        app.UseODataBatching();

        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseForwardedHeaders(new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
        });
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapHub<NotificationHub>("/" + RouteKey + "/hubs/Notification");
        });

    }
}