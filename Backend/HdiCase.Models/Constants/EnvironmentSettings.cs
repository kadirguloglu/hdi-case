
using System.ComponentModel;

public static class EnvironmentSettings
{
    [Description("ASPNETCORE_ENVIRONMENT")]
    public static string ASPNETCORE_ENVIRONMENT => GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");
    [Description("IsDevelopment")]
    public static bool IsDevelopment => ASPNETCORE_ENVIRONMENT == "Development";
    [Description("REDIS_HOST")]
    public static string RedisHost => GetEnvironmentVariable("REDIS_HOST", "localhost");
    [Description("REDIS_PORT")]
    public static string RedisPort => GetEnvironmentVariable("REDIS_PORT", "6379");
    [Description("REDIS_PASSWORD")]
    public static string RedisPassword => GetEnvironmentVariable("REDIS_PASSWORD", "guest");
    [Description("RABBITMQ_HOST")]
    public static string RabbitMqHost => GetEnvironmentVariable("RABBITMQ_HOST", "localhost");
    [Description("RABBITMQ_PORT")]
    public static string RabbitMqPort => GetEnvironmentVariable("RABBITMQ_PORT", "5672");
    [Description("RABBITMQ_USERNAME")]
    public static string RabbitMqUsername => GetEnvironmentVariable("RABBITMQ_USERNAME", "guest");
    [Description("RABBITMQ_PASSWORD")]
    public static string RabbitMqPassword => GetEnvironmentVariable("RABBITMQ_PASSWORD", "guest");
    [Description("JWT_VALID_ISSUER")]
    public static string JWTValidIssuer => GetEnvironmentVariable("JWT_VALID_ISSUER", "https://localhost:7253");
    [Description("JWT_VALID_AUDIENCE")]
    public static string JWTValidAudience => GetEnvironmentVariable("JWT_VALID_AUDIENCE", "https://localhost:7253");
    [Description("JWT_VALID_AUDIENCE")]
    public static string JWTSecret => GetEnvironmentVariable("JWT_VALID_AUDIENCE", "JWTAuthenticationHIGHsecuredPasswordVVVp1OH7Xzyr");
    [Description("JWT_EXPIRES_IN")]
    public static int JWTExpiresIn => Convert.ToInt32(GetEnvironmentVariable("JWT_EXPIRES_IN", "43200"));
    [Description("JWT_REFRESH_TOKEN_EXPIRES_IN")]
    public static int JWTRefreshTokenExpiresIn => Convert.ToInt32(GetEnvironmentVariable("JWT_REFRESH_TOKEN_EXPIRES_IN", "43100"));
    [Description("JWT_VALID_ISSUER")]
    public static string JWTAdminValidIssuer => GetEnvironmentVariable("JWT_VALID_ISSUER", "https://localhost:7253");
    [Description("JWT_VALID_AUDIENCE")]
    public static string JWTAdminValidAudience => GetEnvironmentVariable("JWT_VALID_AUDIENCE", "https://localhost:7253");
    [Description("JWT_VALID_AUDIENCE")]
    public static string JWTAdminSecret => GetEnvironmentVariable("JWT_VALID_AUDIENCE", "JWTAuthenticationHIGHsecuredPasswordVVVp1OH7Xzyr112233");
    [Description("JWT_ADMIN_EXPIRES_IN")]
    public static int JWTAdminExpiresIn => Convert.ToInt32(GetEnvironmentVariable("JWT_ADMIN_EXPIRES_IN", "3200"));
    [Description("JWT_ADMIN_REFRESH_TOKEN_EXPIRES_IN")]
    public static int JWTAdminRefreshTokenExpiresIn => Convert.ToInt32(GetEnvironmentVariable("JWT_ADMIN_REFRESH_TOKEN_EXPIRES_IN", "3100"));
    [Description("SEQ_HOST")]
    public static string SeqHost => GetEnvironmentVariable("SEQ_HOST", "http://localhost:5341");
    [Description("SEQ_USERNAME")]
    public static string SeqUserName => GetEnvironmentVariable("SEQ_USERNAME", "admin");
    [Description("SEQ_PASSWORD")]
    public static string SeqPassword => GetEnvironmentVariable("SEQ_PASSWORD", "admin");
    [Description("LOGGING_IS_ENABLED")]
    public static bool LoggingIsEnabled => GetEnvironmentVariable("LOGGING_IS_ENABLED", "false") == "true";

    public static string GetEnvironmentVariable(string name, string defaultValue = "")
    {
        var value = Environment.GetEnvironmentVariable(name);
        return string.IsNullOrEmpty(value) ? defaultValue : value;
    }
    public static void LogAll()
    {
        Console.WriteLine($"ASPNETCORE_ENVIRONMENT: {ASPNETCORE_ENVIRONMENT}");
        Console.WriteLine($"IsDevelopment: {IsDevelopment}");
        Console.WriteLine($"RedisHost: {RedisHost}");
        Console.WriteLine($"RedisPort: {RedisPort}");
        Console.WriteLine($"RedisPassword: {RedisPassword}");
        Console.WriteLine($"RabbitMqHost: {RabbitMqHost}");
        Console.WriteLine($"RabbitMqPort: {RabbitMqPort}");
        Console.WriteLine($"RabbitMqUsername: {RabbitMqUsername}");
        Console.WriteLine($"RabbitMqPassword: {RabbitMqPassword}");
        Console.WriteLine($"JWTValidIssuer: {JWTValidIssuer}");
        Console.WriteLine($"JWTValidAudience: {JWTValidAudience}");
        Console.WriteLine($"JWTSecret: {JWTSecret}");
        Console.WriteLine($"JWTExpiresIn: {JWTExpiresIn}");
        Console.WriteLine($"JWTRefreshTokenExpiresIn: {JWTRefreshTokenExpiresIn}");
    }
}