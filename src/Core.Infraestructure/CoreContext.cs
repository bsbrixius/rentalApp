using BuildingBlocks.Domain.Base;
using Core.Domain.Aggregates.Driver;
using Core.Domain.Aggregates.Motorcycle;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Core.Infraestructure
{
    public class CoreContext : BaseDb
    {
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
            optionsBuilder.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"));
        }

        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Motorcycle> Motorcycles { get; set; }

    }
}
