using Authentication.Application.DTOs.Auth;
using Authentication.Application.DTOs.User;
using Authentication.Domain;
using BuildingBlocks.Utils;
using System.Net;
using System.Net.Http.Json;
using Testing.Base.Helpers;
using Xunit.Extensions.Ordering;

namespace Authentication.FunctionalTests.Renter
{
    public class UserControllerTests : BaseTest<Program, AuthContext, AuthContextTestingSeeder>, Xunit.IAssemblyFixture<AuthContextTestingSeeder>
    {
        private HttpClient _anonymousClient;

        const string CREATED_USER_ID = nameof(CREATED_USER_ID);
        const string CREATED_USER_EMAIL = nameof(CREATED_USER_EMAIL);
        const string _userFullname = "Renter User Test";
        const string _userPassword = "RenterTest1234";

        public UserControllerTests(TestApplicationFactory<Program, AuthContext, AuthContextTestingSeeder> factory) : base(factory)
        {
            _anonymousClient = Factory.CreateClient();
        }

        [Fact, Order(1)]
        public async Task Anonymous_Pre_Register_Return_Created()
        {
            Data.Add(CREATED_USER_EMAIL, $"{DateTime.UtcNow.ToBinary()}@outlook.com");
            var response = await _anonymousClient.PostAsJsonAsync(API.Renter.User.PreRegister, new PreRegisterUserRenterRequest()
            {
                Email = Data[CREATED_USER_EMAIL].ToString(),
            });
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact, Order(2)]
        public async Task Anonymous_Register_Return_Ok()
        {
            var response = await _anonymousClient.PostAsJsonAsync(API.Renter.User.Register, new RegisterUserRequest()
            {
                Email = Data[CREATED_USER_EMAIL].ToString(),
                BirthDay = DateTime.UtcNow.ToDateOnly().AddYears(-20),
                FullName = _userFullname,
                Password = _userPassword
            });
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact, Order(3)]
        public async Task Anonymous_Auth_Login_Return_Token_Ok()
        {
            var response = await _anonymousClient.PostAsJsonAsync(API.Auth.Login, new LoginRequest()
            {
                Email = Data[CREATED_USER_EMAIL].ToString(),
                Password = _userPassword
            });
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
