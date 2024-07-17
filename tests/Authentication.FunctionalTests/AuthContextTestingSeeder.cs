using Authentication.Domain;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Testing.Base.Helpers;

namespace Authentication.FunctionalTests
{
    public class AuthContextTestingSeeder : BaseDatabaseSeederFixture
    {
        public AuthContextTestingSeeder() : base()
        {

        }

        public override void TrySeedDatabase(IHost host)
        {
            if (!IsInitialized)
            {
                _serviceProvider = host.Services;
                using (var scope = _serviceProvider.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<AuthContext>();
                    //dbContext.Database.EnsureDeleted();
                    dbContext.Database.EnsureCreated();
                    // Seed the database
                    dbContext.TrySeedTestingDatabaseAsync().Wait();
                }
            }
            base.TrySeedDatabase(host);
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
