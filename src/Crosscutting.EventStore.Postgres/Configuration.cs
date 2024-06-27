using Crosscutting.EventStore.Postgres.Service;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Crosscutting.EventStore.Postgres
{
    public static class Configuration
    {
        public static IServiceCollection AddEventStoreForPostgres(this IServiceCollection services)
        {
            services.AddDbContext<EventStoreContext>();
            services.TryAddScoped<IEventStoreService, EventStoreService>();
            return services;
        }
    }
}
