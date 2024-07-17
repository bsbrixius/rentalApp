using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Crosscutting.EventBus.RabbitMq
{

    public static class Configuration
    {
        static bool? _isRunningInContainer;

        static bool IsRunningInContainer => _isRunningInContainer ??= bool.TryParse(Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER"), out var inDocker) && inDocker;
        static bool IsDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";


        public static IServiceCollection AddRabbitMqBroker(this IServiceCollection services, string prefix = "", string hostName = "localhost")
        {
            services.AddMassTransit(x =>
            {
                x.SetKebabCaseEndpointNameFormatter();
                var entryAssembly = Assembly.GetEntryAssembly();
                x.AddConsumers(entryAssembly);
                x.UsingRabbitMq((context, cfg) =>
                {
                    var configuration = context.GetRequiredService<IConfiguration>();

                    var host = configuration.GetSection("RabbitMqHostSettings:Host").Value ?? "localhost";
                    var port = ushort.Parse((configuration.GetSection("RabbitMqHostSettings:Port").Value ?? "5672"));
                    var virtualHost = configuration.GetSection("RabbitMqHostSettings:VirtualHost").Value ?? "/";
                    var username = configuration.GetSection("RabbitMqHostSettings:Username").Value ?? "guest";
                    var password = configuration.GetSection("RabbitMqHostSettings:Password").Value ?? "guest";
                    var connectionString = configuration.GetSection("RabbitMqHostSettings:ConnectionString").Value;


                    if (IsDevelopment)
                        cfg.Host("rabbitmq");
                    else
                    {
                        //cfg.Host(connectionString);

                        cfg.Host(host, port, virtualHost, x =>
                        {
                            x.Username(username);
                            x.Password(password);
                        });
                    }

                    cfg.ConfigureEndpoints(context);
                });
            });

            return services;
        }
    }
}