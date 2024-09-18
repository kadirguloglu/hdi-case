using Microsoft.Extensions.DependencyInjection;
public static class InitCORS
{
    public static void Init(IServiceCollection service)
    {
        service.AddCors(options =>
            {
                if (EnvironmentSettings.IsDevelopment)
                {
                    options.AddDefaultPolicy(
                                    policy =>
                                    {
                                        policy
                                            .WithOrigins(
                                                "http://localhost:3000",
                                                "https://localhost:3000"
                                            )
                                            .SetIsOriginAllowedToAllowWildcardSubdomains()
                                            .AllowAnyHeader()
                                            .AllowAnyMethod()
                                            .AllowCredentials();
                                    });
                }
                else
                {
                    options.AddDefaultPolicy(
                                policy =>
                                {
                                    policy
                                        .WithOrigins(
                                                "https://hdi.com"
                                        )
                                        // .AllowAnyOrigin()
                                        .AllowAnyHeader()
                                        .AllowAnyMethod()
                                        .AllowCredentials();
                                });
                }
            });
    }
}