using Authentication.Domain;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Testing.Base.Helpers;

namespace Authentication.FuncationalTests.Renter
{
    public class UserControllerTests : BaseTest<Program, AuthenticationContext, AuthContextTestingSeeder>, Xunit.IAssemblyFixture<AuthContextTestingSeeder>
    {
        public UserControllerTests(TestApplicationFactory<Program, AuthenticationContext, AuthContextTestingSeeder> factory) : base(factory)
        {
        }
    }
}
