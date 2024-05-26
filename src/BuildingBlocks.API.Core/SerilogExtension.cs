using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

namespace BuildingBlocks.API.Core
{
    public static class SerilogExtension
    {
        public static IHostBuilder AddSerilogCore(this IHostBuilder builder)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .Enrich.WithCorrelationId()
                .Enrich.WithProperty("ApplicationName", $"API Serilog - {Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT")}")
                .WriteTo.Console()
                .WriteTo.File(
                    "logs/ex_.log",
                    LogEventLevel.Error,
                    "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] ({SourceContext}) [{CorrelationId}] {Message:lj}{NewLine}{Exception}",
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 10,
                    rollOnFileSizeLimit: true,
                    fileSizeLimitBytes: 10485760,
                    shared: true
                )
                .WriteTo.File(
                    "logs/dbg_.log",
                    LogEventLevel.Debug,
                    "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] ({SourceContext}) [{CorrelationId}] {Message:lj}",
                     rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 10,
                    rollOnFileSizeLimit: true,
                    fileSizeLimitBytes: 10485760,
                    shared: true
                )
                .CreateLogger();

            builder.UseSerilog(Log.Logger);
            return builder;
        }
    }
}
