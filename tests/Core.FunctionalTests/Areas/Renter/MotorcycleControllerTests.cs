using BuildingBlocks.API.Core.Data.Pagination;
using Core.Application.DTOs.Motorcycle;
using Core.Data;
using System.Net.Http.Json;
using Testing.Base.Helpers;

namespace Core.FunctionalTests.Areas.Renter
{
    public class MotorcycleControllerTests : BaseTest<Program, CoreContext, CoreContextTestingSeeder>, IAssemblyFixture<CoreContextTestingSeeder>
    {
        private HttpClient _renterClient;

        public MotorcycleControllerTests(TestApplicationFactory<Program, CoreContext, CoreContextTestingSeeder> factory) : base(factory)
        {
            _renterClient = Factory.CreateClientWithTestAuth(TestClaimsProvider.WithUserClaims());
        }

        [Fact]
        public async Task Get_Motorcycles_Should_Return_Ok()
        {
            var response = await _renterClient.GetAsync(API.Renter.Motorcycle.Get());
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<PaginatedResult<MotorcycleDTO>>();
            Assert.NotNull(result);
            Assert.True(result.Items.Count() > 0);
        }
    }
}
