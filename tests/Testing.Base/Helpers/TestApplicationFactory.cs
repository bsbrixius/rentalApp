using BuildingBlocks.Domain.Base;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
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

    public class TestApplicationFactory<TStartup, TDatabase> : WebApplicationFactory<TStartup>
        where TStartup : class
        where TDatabase : BaseDb
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
                webHostBuilder.ConfigureTestServices(services =>
                {
                    using (var scope = services.BuildServiceProvider().CreateScope())
                    {
                        var dbContext = scope.ServiceProvider.GetRequiredService<TDatabase>();
                        dbContext.Database.EnsureCreated();
                        //dbContext.TrySeedDevelopmentDatabaseAsync().Wait();
                    }
                });
            });
            return base.CreateHost(hostBuilder);
        }
    }
}