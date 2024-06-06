using Authentication.API.Application.Commands.User.PreRegisterUser;
using Authentication.API.Domain;
using Authentication.API.Infraestructure;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using BuildingBlocks.API.Core.AutofacModules;
using BuildingBlocks.Security;
using BuildingBlocks.Security.Settings;
using MediatR.Extensions.Autofac.DependencyInjection;
using MediatR.Extensions.Autofac.DependencyInjection.Builder;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
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
                options.User.RequireUniqueEmail = true;
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
                //Url = new Uri("")
            };


            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "JWTToken_Auth_API",
                    Version = "v1"
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
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

            //services.AddSwaggerGen(options =>
            //{
            //    options.SwaggerDoc("Portal", new OpenApiInfo
            //    {
            //        Title = "RentalApp Admin - HTTP API",
            //        Version = "v1",
            //        //Contact = contact
            //    });

            //    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            //    {
            //        Description = "Input the JWT like: Bearer {your token}",
            //        Name = "Authorization",
            //        Scheme = "Bearer",
            //        BearerFormat = "JWT",
            //        In = ParameterLocation.Header,
            //        Type = SecuritySchemeType.Http
            //    });

            //    options.AddSecurityRequirement(new OpenApiSecurityRequirement
            //    {
            //        {
            //            new OpenApiSecurityScheme
            //            {
            //                Reference = new OpenApiReference
            //                {
            //                    Type = ReferenceType.SecurityScheme,
            //                    Id = "Bearer"
            //                }
            //            },
            //            Array.Empty<string>()
            //        }
            //    });
            //});

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpContextAccessor();
            services.AddSingleton<JwtUtils<User, Role>>();
            return services;
        }

        public static IHostBuilder UseAutofacIoC(this IHostBuilder hostBuilder)
        {
            var mediatrConfiguration = MediatRConfigurationBuilder
            .Create(typeof(PreRegisterUserCommand).Assembly)
            .WithAllOpenGenericHandlerTypesRegistered()
            .Build();
            hostBuilder.UseServiceProviderFactory(new AutofacServiceProviderFactory());
            hostBuilder.ConfigureContainer<ContainerBuilder>(
               builder =>
               {
                   builder.RegisterModule(new MediatorModule());
                   builder.RegisterMediatR(mediatrConfiguration);
               });

            return hostBuilder;
        }

        public static IServiceCollection AddSecurity(this IServiceCollection services, IConfiguration configuration)
        {
            //TODO Add policies
            services.AddAuthorization();

            var jwtSettingsSection = configuration.GetSection("JwtSettings");
            services.Configure<JwtSettings>(jwtSettingsSection);
            var jwtSettings = jwtSettingsSection.Get<JwtSettings>();
            if (jwtSettings == null) throw new Exception("JwtSettings not found in appsettings.json");
            var key = Encoding.ASCII.GetBytes(jwtSettings.SecretKey);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = true;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = jwtSettings.Audience,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero

                };
            });

            //services.AddJwtAuthentication(configuration);
            //services.AddAuthorization(PoliciesConfiguration.ConfigureAuthorization);
            return services;
        }
    }
}