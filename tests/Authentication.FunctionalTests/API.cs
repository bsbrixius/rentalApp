using System.Text.Json;
using System.Text.Json.Serialization;

namespace Authentication.FunctionalTests
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
            public static class User
            {
                public static string Get(int page = 1, int pageSize = 25) => $"{BaseUrl}/{Area}/user?{nameof(page)}={page}&{nameof(pageSize)}={pageSize}";
                public static string GetById(Guid id) => $"{BaseUrl}/{Area}/user/{id}";
                public static string PreRegister => $"{BaseUrl}/{Area}/user/pre-register";
                public static string Register => $"{BaseUrl}/{Area}/user/register";
            }
        }

        public static class Renter
        {
            private const string Area = "renter";
            public static class User
            {
                public static string Get => $"{BaseUrl}/{Area}/user";
                public static string PreRegister => $"{BaseUrl}/{Area}/user/pre-register";
                public static string Register => $"{BaseUrl}/{Area}/user/register";
            }
        }

        public static class Auth
        {
            public static string Login => $"{BaseUrl}/auth/login";
            public static string Refresh => $"{BaseUrl}/auth/refresh-token";
        }
    }
}
