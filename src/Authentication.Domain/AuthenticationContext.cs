using Authentication.API.Infraestructure;
using Authentication.Domain.Aggregates;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Authentication.Domain
{
    public class AuthenticationContext : AuthenticationBaseContext<User>
    {

        static bool? _isRunningInContainer;
        static bool IsRunningInContainer => _isRunningInContainer ??= bool.TryParse(Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER"), out var inDocker) && inDocker;

        public IConfiguration Configuration { get; }
        const string _schema = "identity";

        public AuthenticationContext(
            IMediator mediator,
            ILogger<AuthenticationContext> logger,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            DbContextOptions<AuthenticationContext> options) : base(mediator, logger, httpContextAccessor, options)
        {
            Configuration = configuration;
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (IsRunningInContainer)
                optionsBuilder.UseNpgsql(Configuration.GetConnectionString("DockerConnection"));
            else
                optionsBuilder.UseNpgsql(Configuration.GetConnectionString("CloudConnection"));
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasDefaultSchema(_schema);
            base.OnModelCreating(builder);
        }
    }
}
