using Authentication.Application.DTOs.Auth;
using Authentication.Application.DTOs.User;
using Authentication.Domain;
using BuildingBlocks.API.Core.Data.Pagination;
using BuildingBlocks.Utils;
using System.Net;
using System.Net.Http.Json;
using Testing.Base.Helpers;
using Xunit.Extensions.Ordering;

namespace Authentication.FunctionalTests.Admin
{
    public class UserControllerTests : BaseTest<Program, AuthContext, AuthContextTestingSeeder>, Xunit.IAssemblyFixture<AuthContextTestingSeeder>
    {
        private HttpClient _anonymousClient;
        private HttpClient _adminClient;
        const string CREATED_USER_ID = nameof(CREATED_USER_ID);
        const string CREATED_USER_EMAIL = nameof(CREATED_USER_EMAIL);
        const string _userFullname = "Admin User Test";
        const string _userPassword = "AdminTest1234";

        const string GET_USER_BY_ID = nameof(GET_USER_BY_ID);


        public UserControllerTests(TestApplicationFactory<Program, AuthContext, AuthContextTestingSeeder> factory) : base(factory)
        {
            _anonymousClient = Factory.CreateClient();
            _adminClient = Factory.CreateClientWithTestAuth(TestClaimsProvider.WithAdminClaims());

        }

        [Fact]
        public async Task Anonymous_Get_Users_Return_Unauthorized()
        {
            var response = await _anonymousClient.GetAsync(API.Admin.User.Get());
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Anonymous_Get_By_Id_Users_Return_Unauthorized()
        {
            var response = await _anonymousClient.GetAsync(API.Admin.User.GetById(Guid.NewGuid()));
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact, Order(1)]
        public async Task Admin_Get_Users_Return_Ok()
        {
            var response = await _adminClient.GetAsync(API.Admin.User.Get());
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var result = await response.Content.ReadFromJsonAsync<PaginatedResult<UserDTO>>();
            Assert.NotNull(result);
            Assert.NotEmpty(result.Items);
            Data.Add(GET_USER_BY_ID, result.Items.First().Id);
        }

        [Fact, Order(2)]
        public async Task Admin_Get_By_Id_User_Return_Ok()
        {
            var response = await _adminClient.GetAsync(API.Admin.User.GetById((Guid)Data[GET_USER_BY_ID]));
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var result = await response.Content.ReadFromJsonAsync<UserDTO>();
            Assert.NotNull(result);
        }

        [Fact, Order(1)]
        public async Task Admin_Pre_Register_Return_Created()
        {
            Data.Add(CREATED_USER_EMAIL, $"{DateTime.UtcNow.ToBinary()}@outlook.com");
            var response = await _adminClient.PostAsJsonAsync(API.Admin.User.PreRegister, new PreRegisterUserRenterRequest()
            {
                Email = Data[CREATED_USER_EMAIL].ToString(),
            });
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact, Order(2)]
        public async Task Admin_Register_Return_Ok()
        {
            var response = await _adminClient.PostAsJsonAsync(API.Admin.User.Register, new RegisterUserRequest()
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
