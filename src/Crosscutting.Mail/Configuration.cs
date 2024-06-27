using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Crosscutting.Mail
{
    public static class Configuration
    {
        public static IServiceCollection AddMailService(this IServiceCollection services)
        {
            services.TryAddScoped<IMailService, MailService>();
            return services;
        }
    }
}
