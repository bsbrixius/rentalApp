using BuildingBlocks.Infrastructure.Base;
using Core.API.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Infrastructure
{
    public class CoreContext : BaseDb
    {
        protected IConfiguration Configuration { get; }

        public CoreContext(IConfiguration configuration, IMediator mediator, ILogger<DbContext> logger, DbContextOptions options) : base(mediator, logger, options)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"));
        }

        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Motorcycle> Motorcycles { get; set; }

    }
}
