using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;

namespace Core.API.FunctionalTests.Helpers
{
    public class TestApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
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
            return base.CreateHost(hostBuilder);
        }
    }
}