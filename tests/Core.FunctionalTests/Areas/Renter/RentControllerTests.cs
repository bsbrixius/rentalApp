using BuildingBlocks.Utils;
using Core.Application.DTOs.Rent;
using Core.Data;
using System.Net.Http.Json;
using Testing.Base.Helpers;

namespace Core.FunctionalTests.Areas.Renter
{
    public class RentControllerTests : BaseTest<Program, CoreContext, CoreContextTestingSeeder>, Xunit.IAssemblyFixture<CoreContextTestingSeeder>
    {
        private HttpClient _renterClient;

        public RentControllerTests(TestApplicationFactory<Program, CoreContext, CoreContextTestingSeeder> factory) : base(factory)
        {
            _renterClient = Factory.CreateClientWithTestAuth(TestClaimsProvider.WithUserClaims());
        }

        [Fact]
        public async Task Get_Plans_Return_Ok()
        {
            var response = await _renterClient.GetAsync(API.Renter.Rent.GetPlan);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<List<RentPlanDTO>>();
            Assert.NotNull(result);
            Assert.True(result.Count() > 0);
        }

        [Fact]
        public async Task Get_Rent_Information_NoPenalty_Return_Ok()
        {
            var startAt = DateTime.Today.ToDateOnly();
            var endAt = DateTime.Today.AddDays(15).ToDateOnly();
            var expectedReturnAt = endAt;
            var response = await _renterClient.GetAsync(API.Renter.Rent.GetInformation(startAt, endAt, expectedReturnAt));

            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<RentInformationDTO>();
            Assert.NotNull(result);
            Assert.Null(result.PenaltyPriceInCents);
        }

        [Fact]
        public async Task Get_Rent_Information_WithPenalty_Return_Ok()
        {
            var startAt = DateTime.Today.ToDateOnly();
            var endAt = DateTime.Today.AddDays(15).ToDateOnly();
            var expectedReturnAt = endAt.AddDays(-5);
            var response = await _renterClient.GetAsync(API.Renter.Rent.GetInformation(startAt, endAt, expectedReturnAt));

            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<RentInformationDTO>();
            Assert.NotNull(result);
            Assert.NotNull(result.PenaltyPriceInCents);
        }
    }
}
