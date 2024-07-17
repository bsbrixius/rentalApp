using Authentication.Application.Commands.User.PreRegisterUser;
using Authentication.Domain;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using BuildingBlocks.API.Core.AutofacModules;
using BuildingBlocks.API.Core.Security;
using BuildingBlocks.API.Core.Swagger;
using BuildingBlocks.Identity.Configuration;
using BuildingBlocks.Identity.Services;
using BuildingBlocks.Infrastructure.Filters;
using BuildingBlocks.Security;
using MediatR.Extensions.Autofac.DependencyInjection;
using MediatR.Extensions.Autofac.DependencyInjection.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Authentication.API
{
    public static class ProgramExtensions
    {
        public static IServiceCollection AddWebAppConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddRouting(options =>
            {
                options.LowercaseUrls = true;
                options.LowercaseQueryStrings = true;
            });

            services
                .AddControllers(options =>
                {
                    options.AllowEmptyInputInBodyModelBinding = true;
                    //options.Filters.Add(typeof(DomainExceptionFilter));
                    //options.Filters.Add(typeof(ValidateModelStateFilter));
                    //options.Filters.Add(new AuthorizeFilter(Policies.NotAnonymous));
                    //options.Filters.Add<AccessAuthorizeAttribute>();
                    options.Filters.Add(typeof(HttpGlobalExceptionFilter));
                    options.Filters.Add(new ProducesAttribute("application/json"));
                })
                .AddApplicationPart(typeof(Program).Assembly)
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                    options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
                });

            services.Configure<ApiBehaviorOptions>(options => options.SuppressInferBindingSourcesForParameters = true);

            //services.AddFluentValidationAutoValidation();
            //services.AddApiVersioning(config =>
            //{
            //    config.DefaultApiVersion = new ApiVersion(1, 0);
            //    config.AssumeDefaultVersionWhenUnspecified = true;
            //    config.ReportApiVersions = true;
            //    config.ApiVersionReader = new HeaderApiVersionReader("x-api-version");
            //});
            return services;
        }

        public static IServiceCollection AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
        {
            var hcBuilder = services.AddHealthChecks();

            hcBuilder.AddCheck("self", () => HealthCheckResult.Healthy());

            return services;
        }


        public static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentityBase<Authentication.Domain.Aggregates.User, AuthContext>();

            return services;
        }
        public class ControllerInfo
        {
            public Type ControllerType { get; set; }
            public ApiExplorerSettingsAttribute ApiExplorerSettings { get; set; }
            public AreaAttribute Area { get; set; }
        }

        public static IServiceCollection AddSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            var contact = new OpenApiContact
            {
                Name = "RentalApp",
                Email = "brunobrixius@outlook.com",
                //Url = new Uri("")
            };


            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {

                c.DocumentFilter<LowercaseDocumentFilter>();
                c.SwaggerDocByArea();

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                    new OpenApiSecurityScheme {
                        Reference = new OpenApiReference {
                            Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                        }
                    },
                    new string[] {}
                    }
                });
            });
            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddResponseCaching();
            services.AddHttpContextAccessor();
            services.TryAddScoped<IIdentityService, IdentityService>();
            services.AddSingleton<JwtBuilder<Authentication.Domain.Aggregates.User>>();
            services.AddSingleton<JwtValidator>();
            return services;
        }

        public static IHostBuilder UseAutofacIoC(this IHostBuilder hostBuilder, IConfiguration configuration)
        {
            var mediatrConfiguration = MediatRConfigurationBuilder
            .Create(typeof(PreRegisterUserCommand).Assembly)
            .WithAllOpenGenericHandlerTypesRegistered()
            .Build();
            hostBuilder.UseServiceProviderFactory(new AutofacServiceProviderFactory());
            hostBuilder.ConfigureContainer<ContainerBuilder>(
               builder =>
               {
                   builder.RegisterModule(new AppModule(configuration));
                   builder.RegisterMediatR(mediatrConfiguration);
               });

            return hostBuilder;
        }

        public static IServiceCollection AddSecurity(this IServiceCollection services, IConfiguration configuration)
        {
            //TODO Add policies
            services.AddJwtAuthenticationConfiguration(configuration);
            services.AddAuthorization(PoliciesConfiguration.ConfigureAuthorization);
            return services;
        }
    }
}