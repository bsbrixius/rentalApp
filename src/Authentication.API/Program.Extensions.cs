using Authentication.Domain.Domain;
using Authentication.Infraestructure;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using BuildingBlocks.API.Core.AutofacModules;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using System.Text.Json;

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
                    //options.Filters.Add(typeof(DomainExceptionFilter));
                    //options.Filters.Add(typeof(ValidateModelStateFilter));
                    //options.Filters.Add(new AuthorizeFilter(Policies.NotAnonymous));
                    options.Filters.Add(new ProducesAttribute("application/json"));
                })
                .AddApplicationPart(typeof(Program).Assembly)
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                    options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
                });
            services.AddResponseCaching();

            //services.AddEndpointsApiExplorer();

            services.Configure<ApiBehaviorOptions>(options => options.SuppressInferBindingSourcesForParameters = true);

            //services.AddFluentValidationAutoValidation();

            return services;
        }

        public static IServiceCollection AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
        {
            var hcBuilder = services.AddHealthChecks();

            hcBuilder.AddCheck("self", () => HealthCheckResult.Healthy());

            //hcBuilder
            //    .AddSqlServer(
            //        configuration.GetConnectionString(""),
            //        name: "",
            //        tags: Array.Empty<string>());

            return services;
        }


        public static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddIdentity<User, Role>(options =>
            {
                options.SignIn.RequireConfirmedEmail = false;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequiredLength = 0;
                options.Lockout.MaxFailedAccessAttempts = 5;
            })
                .AddEntityFrameworkStores<AuthenticationContext>()
                .AddApiEndpoints()
                .AddDefaultTokenProviders();

            // Add services to the container.
            services.AddDbContext<AuthenticationContext>();
            //builder.Services.AddDbContext<AuthenticationContext>(options => options.UseInMemoryDatabase(nameof(AuthenticationContext)));

            return services;
        }

        public static IServiceCollection AddSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            var contact = new OpenApiContact
            {
                Name = "RentalApp",
                Email = "brunobrixius@outlook.com",
                Url = new Uri("")
            };


            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("Portal", new OpenApiInfo
                {
                    Title = "RentalApp Admin - HTTP API",
                    Version = "v1",
                    Contact = contact
                });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Description = "Input the JWT like: Bearer {your token}",
                    Name = "Authorization",
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {

            return services;
        }

        public static IServiceCollection AddIoC(this IServiceCollection services, IConfiguration configuration)
        {
            var container = new ContainerBuilder();
            container.Populate(services);
            container.RegisterModule(new MediatorModule());

            return services;
        }

        public static IServiceCollection AddSecurity(this IServiceCollection services, IConfiguration configuration)
        {

            services
                .AddAuthorization()
                .AddAuthentication(IdentityConstants.BearerScheme)
                .AddBearerToken(IdentityConstants.BearerScheme);

            //services.AddJwtAuthentication(configuration);
            //services.AddAuthorization(PoliciesConfiguration.ConfigureAuthorization);

            return services;
        }
    }
}