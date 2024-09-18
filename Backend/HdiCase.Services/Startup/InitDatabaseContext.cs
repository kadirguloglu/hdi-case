using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using BN = BCrypt.Net;

public static class InitDatabaseContext
{
    public static void Init(IServiceCollection service)
    {
        service
                .AddDbContext<HdiDbContext>(options =>
                {
                    options.UseChangeTrackingProxies(false, false);
                    options.UseLazyLoadingProxies(false);
                    if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
                    {
                        options.EnableSensitiveDataLogging();
                    }
                    options.UseSqlServer(EnvironmentSettings.MssqlConnectionString, y =>
                    {
                        y.MigrationsAssembly("HdiCase.RestApi");
                        y.CommandTimeout(1200);
                        y.EnableRetryOnFailure(10, TimeSpan.FromSeconds(30), null);
                    });

                }, ServiceLifetime.Singleton);
        using (HdiDbContext context = new HdiDbContext())
        {
            context.Database.Migrate();
        }
    }
}