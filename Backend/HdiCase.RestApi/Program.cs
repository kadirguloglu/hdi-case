using Humanizer;
using Serilog;
using Serilog.Events;

namespace HdiCase.RestApi
{
    public class Program
    {
        private static readonly string RouteKey = Enum_RoutingKeys.Api.ToString().Underscore();
        public static void Main(string[] args)
        {
            try
            {
                Log.Logger = new LoggerConfiguration()
                    .Filter.ByExcluding(c =>
                    {
                        return IgnoredExceptions.Ignore(c);
                    })
                    .WriteTo.Seq(EnvironmentSettings.SeqHost)
                    .MinimumLevel.Information()
                    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                    .Enrich.WithProperty("Service Name", RouteKey)
                    .Enrich.WithProperty("Environment", EnvironmentSettings.ASPNETCORE_ENVIRONMENT)
                    .CreateLogger();
                Log.Information("Starting web host = " + RouteKey);
                CreateHostBuilder(args).Build().Run();

            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly = " + RouteKey);
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureKestrel(serverOptions =>
                    {
                        serverOptions.ListenAnyIP(7050, listenOptions =>
                                            listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http1);

                        // http2 grpc port
                        serverOptions.ListenAnyIP(7060, listenOptions =>
                            listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2);
                    });
                    webBuilder.UseStartup<Startup>();
                })
                .UseSerilog();
    }
}