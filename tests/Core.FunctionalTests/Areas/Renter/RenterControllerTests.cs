using Core.Data;
using Testing.Base.Helpers;

namespace Core.FunctionalTests.Areas.Renter
{
    public class RenterControllerTests : BaseTest<Program, CoreContext, CoreContextTestingSeeder>, IAssemblyFixture<CoreContextTestingSeeder>
    {
        private HttpClient _renterClient;

        public RenterControllerTests(TestApplicationFactory<Program, CoreContext, CoreContextTestingSeeder> factory) : base(factory)
        {
            _renterClient = Factory.CreateClientWithTestAuth(TestClaimsProvider.WithUserClaims());
        }

        //[Fact]
        //public async Task Get_Renter_Return_Ok()
        //{
        //    var response = await _renterClient.GetAsync(API.Renter.RenterController.Get);
        //    response.EnsureSuccessStatusCode();
        //    var result = await response.Content.ReadFromJsonAsync<RenterDTO>();
        //    Assert.NotNull(result);
        //}
    }
}
