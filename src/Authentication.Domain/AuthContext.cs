using Authentication.API.Infraestructure;
using Authentication.Domain.Aggregates;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Authentication.Domain
{
    public class AuthContext : AuthenticationBaseContext<User>
    {

        public IConfiguration Configuration { get; }
        const string _schema = "identity";

        public AuthContext(
            IMediator mediator,
            ILogger<AuthContext> logger,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            DbContextOptions<AuthContext> options) : base(mediator, logger, httpContextAccessor, options)
        {
            Configuration = configuration;
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"));
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasDefaultSchema(_schema);
            base.OnModelCreating(builder);
        }
    }
}
