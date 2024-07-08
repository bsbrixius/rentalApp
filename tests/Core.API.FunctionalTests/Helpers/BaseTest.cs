using AutoFixture;
using Bogus;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Core.API.FunctionalTests.Helpers
{
    public class BaseTest : IClassFixture<TestApplicationFactory<Program>>
    {
        protected Faker Faker { get; } = new Faker();
        protected Fixture Fixture { get; } = new Fixture();
        protected WebApplicationFactory<Program> Factory { get; }
        protected Dictionary<string, object> Data { get; }

        public BaseTest(TestApplicationFactory<Program> factory)
        {
            Factory = factory;
            Data = factory.Data;
        }

        // Add you other helper methods here
    }
}
