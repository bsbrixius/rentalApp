﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;

namespace Core.API.FunctionalTests.Helpers
{
    public static class WebApplicationFactoryExtensions
    {
        public static WebApplicationFactory<T> WithAuthentication<T>(this WebApplicationFactory<T> factory, TestClaimsProvider claimsProvider) where T : class
        {
            return factory.WithWebHostBuilder(hostBuilder =>
            {
                hostBuilder.ConfigureTestServices(services =>
                {
                    services
                        .AddAuthentication(x =>
                        {
                            x.DefaultAuthenticateScheme = TestAuthHandler.AuthenticationScheme;
                            x.DefaultScheme = TestAuthHandler.AuthenticationScheme;
                        })
                        .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(TestAuthHandler.AuthenticationScheme, options => { });

                    //services.AddAuthentication(TestAuthHandler.AuthenticationScheme)
                    //        .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(TestAuthHandler.AuthenticationScheme, op => { });
                    services.AddScoped<TestClaimsProvider>(_ => claimsProvider);
                });
            });
        }

        public static HttpClient CreateClientWithTestAuth<T>(this WebApplicationFactory<T> factory, TestClaimsProvider claimsProvider) where T : class
        {
            var client = factory.WithAuthentication(claimsProvider).CreateClient();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(TestAuthHandler.AuthenticationScheme);

            return client;
        }
    }
}