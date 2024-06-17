using Authentication.API.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Authentication.API.Infraestructure
{
    public class AuthenticationContext : AuthenticationBaseContext<User>
    {
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
            optionsBuilder.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"));
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasDefaultSchema(_schema);
            base.OnModelCreating(builder);
        }
    }
}
