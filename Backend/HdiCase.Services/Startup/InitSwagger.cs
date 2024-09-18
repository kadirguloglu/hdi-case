using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

public static class InitSwagger
{
    public static void Init(IServiceCollection service)
    {
        if (EnvironmentSettings.IsDevelopment)
        {
            service.AddApiVersioning(setup =>
            {
                setup.DefaultApiVersion = new ApiVersion(1, 0);
                setup.AssumeDefaultVersionWhenUnspecified = true;
                setup.ReportApiVersions = true;
            });
            service.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("Api-v1", new OpenApiInfo
                {
                    Title = "",
                    Version = "v1",
                    Description = "",
                    Contact = new OpenApiContact
                    {
                        Name = "."
                    },
                });
                c.SwaggerDoc("Api-v1", new OpenApiInfo
                {
                    Title = "",
                    Version = "v1",
                    Description = "",
                    Contact = new OpenApiContact
                    {
                        Name = "."
                    },
                });
                c.SwaggerDoc("OData-v1", new OpenApiInfo
                {
                    Title = "",
                    Version = "v1",
                    Description = "",
                    Contact = new OpenApiContact
                    {
                        Name = "."
                    },
                });
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                });
                // c.CustomSchemaIds(x =>
                // {
                //     return $"{Guid.NewGuid().ToString("n")}-{x.Name}";
                // });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement{{
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] {}}});
            });
        }
    }
}