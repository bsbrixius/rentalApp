using Bogus;
using BuildingBlocks.API.Core.Data.Pagination;
using Core.Application.DTOs.Motorcycle;
using Core.Data;
using System.Net;
using System.Net.Http.Json;
using Testing.Base.Helpers;
using Xunit.Extensions.Ordering;

namespace Core.FunctionalTests.Areas.Admin
{
    public class MotorcycleControllerTests : BaseTest<Program, CoreContext>
    {
        private HttpClient _adminClient;
        private HttpClient _renterClient;
        const string CREATED_MOTORCYCLE_ID = nameof(CREATED_MOTORCYCLE_ID);
        const string CREATED_MOTORCYCLE_PLATE = nameof(CREATED_MOTORCYCLE_PLATE);
        const string UPDATED_MOTORCYCLE_PLATE = nameof(UPDATED_MOTORCYCLE_PLATE);
        const string DUPLICATED_MOTORCYCLE_PLATE = nameof(DUPLICATED_MOTORCYCLE_PLATE);

        public MotorcycleControllerTests(TestApplicationFactory<Program, CoreContext> factory) : base(factory)
        {
            _adminClient = Factory.CreateClientWithTestAuth(TestClaimsProvider.WithAdminClaims());
            _renterClient = Factory.CreateClientWithTestAuth(TestClaimsProvider.WithUserClaims());
        }

        [Fact]
        public async Task Admin_Get_Motorcycles_Should_Return_Ok()
        {
            var response = await _adminClient.GetAsync(API.Admin.Motorcycle.Get());
            response.EnsureSuccessStatusCode();
        }
        #region Admin

        [Fact, Order(1)]
        public async Task Admin_Create_Motorcycles_Should_Return_Ok()
        {
            Data.Add(DUPLICATED_MOTORCYCLE_PLATE, DataGenerator.GenerateNewPlate());
            var request = new RegisterMotorcycleRequest
            {
                Model = $"Model-{DateTime.UtcNow.ToBinary().ToString()}",
                Plate = Data[DUPLICATED_MOTORCYCLE_PLATE].ToString(),
                Year = (uint)Faker.Date.Random.Number(2000, 2022)
            };
            var response = await _adminClient.PostAsJsonAsync(API.Admin.Motorcycle.Post, request);
            Data.Add(CREATED_MOTORCYCLE_PLATE, request.Plate);
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }
        [Fact, Order(2)]
        public async Task Admin_Create_Motorcycles_Duplicated_Plate_Should_Return_Error()
        {
            var request = new RegisterMotorcycleRequest
            {
                Model = $"Model-{DateTime.UtcNow.ToBinary().ToString()}",
                Plate = Data[DUPLICATED_MOTORCYCLE_PLATE].ToString(),
                Year = (uint)Faker.Date.Random.Number(2000, 2022)
            };
            var response = await _adminClient.PostAsJsonAsync(API.Admin.Motorcycle.Post, request);
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }

        [Fact, Order(3)]
        public async Task Admin_Get_Create_Motorcycles_Should_Return_Ok()
        {
            var response = await _adminClient.GetAsync(API.Admin.Motorcycle.Get(plate: Data[CREATED_MOTORCYCLE_PLATE].ToString()));
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<PaginatedResult<MotorcycleDTO>>();
            Assert.NotNull(result);
            Assert.True(result.Items.Count() == 1);
            Assert.True(result.Items.First().Plate == Data[CREATED_MOTORCYCLE_PLATE].ToString());
            Data.Add(CREATED_MOTORCYCLE_ID, result.Items.First().Id);
        }

        [Fact, Order(4)]
        public async Task Admin_Update_Motorcycles_Should_Return_Ok()
        {
            var newPlate = DataGenerator.GenerateNewPlate();
            Data.Add(UPDATED_MOTORCYCLE_PLATE, newPlate);
            var request = new UpdateMotorcyclePlateRequest
            {
                Plate = newPlate
            };
            var response = await _adminClient.PatchAsJsonAsync(API.Admin.Motorcycle.PatchPlate(Data[CREATED_MOTORCYCLE_ID].ToString()), request);
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            response = await _adminClient.GetAsync(API.Admin.Motorcycle.Get(plate: Data[CREATED_MOTORCYCLE_PLATE].ToString()));
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<PaginatedResult<MotorcycleDTO>>();
            Assert.NotNull(result);
            Assert.Empty(result.Items);

            response = await _adminClient.GetAsync(API.Admin.Motorcycle.Get(plate: Data[UPDATED_MOTORCYCLE_PLATE].ToString()));
            response.EnsureSuccessStatusCode();
            result = await response.Content.ReadFromJsonAsync<PaginatedResult<MotorcycleDTO>>();
            Assert.NotNull(result);
            Assert.True(result.Items.Count() == 1);
            Assert.True(result.Items.First().Plate == Data[UPDATED_MOTORCYCLE_PLATE].ToString());
        }

        [Fact, Order(5)]
        public async Task Admin_Delete_Motorcycles_Should_Return_Ok()
        {
            var response = await _adminClient.DeleteAsync(API.Admin.Motorcycle.Delete(Data[CREATED_MOTORCYCLE_ID].ToString()));
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            response = await _adminClient.GetAsync(API.Admin.Motorcycle.Get(plate: Data[UPDATED_MOTORCYCLE_PLATE].ToString()));
            var result = await response.Content.ReadFromJsonAsync<PaginatedResult<MotorcycleDTO>>();
            Assert.NotNull(result);
            Assert.Empty(result.Items);
        }
        #endregion

        #region Renter
        [Fact]
        public async Task Renter_Get_Motorcycles_Should_Return_Forbidden()
        {
            var response = await _renterClient.GetAsync(API.Admin.Motorcycle.Get());
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task Renter_Create_Motorcycles_Should_Return_Forbidden()
        {
            var request = new RegisterMotorcycleRequest
            {
                Model = Faker.Company.CompanyName(),
                Plate = DataGenerator.GenerateNewPlate(),
                Year = (uint)Faker.Date.Random.Number(2000, 2022)
            };
            var response = await _renterClient.PostAsJsonAsync(API.Admin.Motorcycle.Post, request);
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task Renter_Update_Motorcycles_Should_Return_Forbidden()
        {
            var newPlate = DataGenerator.GenerateNewPlate();
            var request = new UpdateMotorcyclePlateRequest
            {
                Plate = newPlate
            };
            var response = await _renterClient.PatchAsJsonAsync(API.Admin.Motorcycle.PatchPlate(Guid.NewGuid().ToString()), request);
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task Renter_Delete_Motorcycles_Should_Return_Forbidden()
        {
            var response = await _renterClient.DeleteAsync(API.Admin.Motorcycle.Delete(Guid.NewGuid().ToString()));
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        #endregion
    }
}
