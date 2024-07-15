using AutoFixture;
using Bogus;
using BuildingBlocks.Domain.Base;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Testing.Base.Helpers
{
    public class BaseTest<TStartup> : IClassFixture<TestApplicationFactory<TStartup>>
        where TStartup : class
    {
        protected Faker Faker { get; } = new Faker();
        protected Fixture Fixture { get; } = new Fixture();
        protected WebApplicationFactory<TStartup> Factory { get; }
        protected Dictionary<string, object> Data { get; }

        public BaseTest(TestApplicationFactory<TStartup> factory)
        {
            Factory = factory;
            Data = factory.Data;
        }

        // Add you other helper methods here
    }

    public class BaseTest<TStartup, TDatabase> : IClassFixture<TestApplicationFactory<TStartup, TDatabase>>
        where TStartup : class
        where TDatabase : BaseDb
    {
        protected Faker Faker { get; } = new Faker();
        protected Fixture Fixture { get; } = new Fixture();
        protected WebApplicationFactory<TStartup> Factory { get; }
        protected Dictionary<string, object> Data { get; }

        public BaseTest(TestApplicationFactory<TStartup, TDatabase> factory)
        {
            Factory = factory;
            Data = factory.Data;
        }

        // Add you other helper methods here
    }
}
