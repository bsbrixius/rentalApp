using BuildingBlocks.Domain.Base;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;

namespace Testing.Base.Helpers
{
    public class TestApplicationFactory<TStartup> : WebApplicationFactory<TStartup>
        where TStartup : class
    {
        public Dictionary<string, object> Data { get; set; }
        private readonly string _environment = "Testing";

        public TestApplicationFactory() : base()
        {
            Data = new Dictionary<string, object>();
        }

        protected override IHost CreateHost(IHostBuilder hostBuilder)
        {
            hostBuilder.UseEnvironment(_environment);
            hostBuilder.ConfigureWebHost(webHostBuilder =>
            {
                webHostBuilder.UseTestServer();
            });
            return base.CreateHost(hostBuilder);
        }
    }

    public class TestApplicationFactory<TStartup, TDatabase, TBaseDatabaseSeeder> : WebApplicationFactory<TStartup>
        where TStartup : class
        where TDatabase : BaseDb
        where TBaseDatabaseSeeder : BaseDatabaseSeederFixture, new()
    {
        public Dictionary<string, object> Data { get; set; }
        private readonly string _environment = "Testing";
        protected TBaseDatabaseSeeder _databaseSeeder;

        public TestApplicationFactory(TBaseDatabaseSeeder baseDatabaseSeeder) : base()
        {
            Data = new Dictionary<string, object>();
            _databaseSeeder = baseDatabaseSeeder;
        }

        protected override IHost CreateHost(IHostBuilder hostBuilder)
        {
            hostBuilder.UseEnvironment(_environment);
            var localHostBuilder = base.CreateHost(hostBuilder);
            _databaseSeeder.TrySeedDatabase(localHostBuilder);
            return localHostBuilder;
        }
    }

}