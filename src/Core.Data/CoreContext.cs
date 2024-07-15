using BuildingBlocks.Domain.Base;
using Core.Domain.Aggregates.Motorcycle;
using Core.Domain.Aggregates.Rent;
using Core.Domain.Aggregates.Renter;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Core.Data
{
    public class CoreContext : BaseDb
    {
        static bool? _isRunningInContainer;
        static bool IsRunningInContainer => _isRunningInContainer ??= bool.TryParse(Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER"), out var inDocker) && inDocker;

        protected IConfiguration Configuration { get; }
        public CoreContext(
            IConfiguration configuration,
            IMediator mediator,
            ILogger<DbContext> logger,
            IHttpContextAccessor httpContextAccessor,
            DbContextOptions options
            ) : base(mediator, logger, httpContextAccessor, options)
        {
            Configuration = configuration;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CoreContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (IsRunningInContainer)
                optionsBuilder.UseNpgsql(Configuration.GetConnectionString("DockerConnection"));
            else
                optionsBuilder.UseNpgsql(Configuration.GetConnectionString("CloudConnection"));
        }

        public DbSet<Rent> Rents { get; set; }
        public DbSet<Renter> Renters { get; set; }
        public DbSet<Motorcycle> Motorcycles { get; set; }

    }
}
