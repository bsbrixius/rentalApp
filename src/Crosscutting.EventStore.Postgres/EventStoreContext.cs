using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Crosscutting.EventStore.Postgres
{
    public class EventStoreContext : DbContext
    {
        public IConfiguration Configuration { get; }

        public EventStoreContext(IConfiguration configuration, DbContextOptions options) : base(options)
        {
            Configuration = configuration;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(EventStoreContext).Assembly);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = Configuration.GetConnectionString("EventStoreConnection");
            ArgumentNullException.ThrowIfNull(connectionString, nameof(connectionString));
            optionsBuilder.UseNpgsql(connectionString);
        }
    }
}
