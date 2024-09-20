using System.Net;
using System.Text.Json.Serialization;
using System.Threading.RateLimiting;
using Humanizer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.OData;

public class Startup
{
    // extra bir route url eklememizin sebebi microservice projelerimizde her bir servisi alt bir domainde degilde ayni domain uzerinde yayinlamak istersek
    // her bir servis icin ozel bir key vererek birbirinden ayristirmak icin kullaniyoruz
    private readonly string RouteKey = Enum_RoutingKeys.Api.ToString().Underscore();
    public void ConfigureServices(IServiceCollection services)
    {
        InitDatabaseContext.Init(services);
        var redisHost = EnvironmentSettings.RedisHost;
        var redisPort = EnvironmentSettings.RedisPort;
        var redisPassword = EnvironmentSettings.RedisPassword;
        services.AddRateLimiter(options =>
        {
            // too many request hatasi donuyoruz.
            options.RejectionStatusCode = 429;
            options.AddPolicy("ApiKeyPolicy", context =>
            {
                // API anahtarını isteğin başlığından alın
                var path = context.Request.Path.Value;
                Console.WriteLine("request path = " + path);
                if (path != null && path.ToLowerInvariant().StartsWith("/api/api/v1/Aggrement/AddNewAggrement"))
                {
                    // API anahtarını isteğin başlığından alın
                    var apiKey = context.Request.Headers["X-Api-Key"].ToString();

                    if (string.IsNullOrEmpty(apiKey))
                    {
                        // API anahtarı yoksa erişimi engelle
                        return RateLimitPartition.GetFixedWindowLimiter("NoApiKey", _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 0, // İzin verilen istek sayısı 0
                            Window = TimeSpan.FromMinutes(1),
                            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                            QueueLimit = 0
                        });
                    }
                    var dbContext = new HdiDbContext();
                    var company = dbContext.Company.FirstOrDefault(x => x.ApiKey == apiKey);
                    if (company is null || !company.ApiIsActive)
                    {
                        // company bulunamazsa veya api aktif degilse engelle
                        return RateLimitPartition.GetFixedWindowLimiter("NotfoundApiKey", _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 0, // İzin verilen istek sayısı 0
                            Window = TimeSpan.FromMinutes(1),
                            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                            QueueLimit = 0
                        });
                    }

                    // Her API anahtarı için ayrı bir rate limiter oluştur
                    return RateLimitPartition.GetFixedWindowLimiter(apiKey, _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = company.ApiPerMinuteMaximumRequestCount, // Dakikada en fazla 10 istek
                        Window = TimeSpan.FromMinutes(1), // 1 dakikalik
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst, // en eski isi ilk basta yurut
                        QueueLimit = 0 // siraya islem alma
                    });
                }
                else
                {
                    // Diğer URL'ler için rate limiting uygulamayın
                    return RateLimitPartition.GetNoLimiter(string.Empty);
                }
            });
        });

        services.AddSignalR(x =>
        {
            if (EnvironmentSettings.IsDevelopment)
            {
                x.EnableDetailedErrors = true;
            }
        })
        .AddStackExchangeRedis(options => // signalr redis backplane configuration
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
        }).AddJsonOptions(x =>
        {
            x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
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
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseRateLimiter();
        // load balancing header redirect configuration
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