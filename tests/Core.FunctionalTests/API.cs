using BuildingBlocks.Common.Enums;
using System.Text.Json;
using System.Text.Json.Serialization;
using static Core.Application.DTOs.Motorcycle.SearchMotorcycleRequest;

namespace Core.FunctionalTests
{
    public static class API
    {
        public static JsonSerializerOptions JsonSerializerSettings
        {
            get
            {
                var jsonSerializerOptions = new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                };

                jsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());

                return jsonSerializerOptions;
            }
        }
        private const string BaseUrl = "api/v1";
        public static class Admin
        {
            private const string Area = "admin";
            public static class Motorcycle
            {
                public static string Get(string? plate = null, OrderByType orderByType = OrderByType.Year, SortByType sortByType = SortByType.Descending) => $"{BaseUrl}/{Area}/motorcycle?{nameof(plate)}={plate}&{nameof(orderByType)}={orderByType}&{nameof(sortByType)}={sortByType}";
                public static string Post => $"{BaseUrl}/{Area}/motorcycle";
                public static string Put => $"{BaseUrl}/{Area}/motorcycle";
                public static string PatchPlate(string id) => $"{BaseUrl}/{Area}/motorcycle/{id}/plate";
                public static string Delete(string id) => $"{BaseUrl}/{Area}/motorcycle/{id}";
            }
        }

        public static class Renter
        {
            private const string Area = "renter";
            public static class Motorcycle
            {
                public static string Get(OrderByType orderByType = OrderByType.Year, SortByType sortByType = SortByType.Descending) => $"{BaseUrl}/{Area}/motorcycle?{nameof(orderByType)}={orderByType}&{nameof(sortByType)}={sortByType}";
            }
            public static class Rent
            {
                public static string GetPlan => $"{BaseUrl}/{Area}/rent/plans";
                public static string GetInformation(DateOnly startAt, DateOnly endAt, DateOnly expectedReturnAt) => $"{BaseUrl}/{Area}/rent/information?{nameof(startAt)}={startAt}&{nameof(endAt)}={endAt}&{nameof(expectedReturnAt)}={expectedReturnAt}";
                public static string PostRent(DateOnly endAt, DateOnly expectedReturnAt) => $"{BaseUrl}/{Area}/rent/information?{nameof(endAt)}={endAt}&{nameof(expectedReturnAt)}={expectedReturnAt}";
            }

            public static class RenterController
            {
                public static string Get => $"{BaseUrl}/{Area}/renter";
                //public static string GetCNH => $"{BaseUrl}/{Area}/renter/cnh";
                //public static string Register => $"{BaseUrl}/{Area}/renter/register";
            }
        }
    }
}
