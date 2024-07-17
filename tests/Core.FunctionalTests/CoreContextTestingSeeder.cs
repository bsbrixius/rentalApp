using Core.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Testing.Base.Helpers;

namespace Core.FunctionalTests
{
    public class CoreContextTestingSeeder : BaseDatabaseSeederFixture
    {
        public CoreContextTestingSeeder() : base()
        {
        }

        public override void Dispose()
        {
            base.Dispose();
        }

        public override void TrySeedDatabase(IHost host)
        {
            if (!IsInitialized)
            {
                _serviceProvider = host.Services;
                using (var scope = _serviceProvider.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<CoreContext>();
                    //dbContext.Database.EnsureDeleted();
                    dbContext.Database.EnsureCreated();
                    // Seed the database
                    dbContext.TrySeedTestingDatabaseAsync().Wait();
                }
            }
            base.TrySeedDatabase(host);
        }
    }
}
