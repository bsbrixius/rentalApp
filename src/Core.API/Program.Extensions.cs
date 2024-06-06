using Autofac;
using Autofac.Extensions.DependencyInjection;
using BuildingBlocks.API.Core.AutofacModules;
using BuildingBlocks.Security;
using Core.API.Application.Commands.Motorcycle.RegisterMotorcycle;
using Core.Infrastructure;
using MediatR.Extensions.Autofac.DependencyInjection;
using MediatR.Extensions.Autofac.DependencyInjection.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using System.Text.Json;

namespace Core.API
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
            services.AddDbContext<CoreContext>();
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

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();
            services.AddHttpContextAccessor();
            services.AddSingleton<JwtValidator>();
            //builder.Services.AddRabbitMqBroker();
            return services;
        }

        public static IHostBuilder UseAutofacIoC(this IHostBuilder hostBuilder)
        {
            var mediatrConfiguration = MediatRConfigurationBuilder
            .Create(typeof(RegisterMotorcycleCommand).Assembly)
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

        public static IHostBuilder UseSerilogCore(this IHostBuilder builder)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .Enrich.WithCorrelationId()
                .Enrich.WithProperty("ApplicationName", $"API Serilog - {Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT")}")
                .WriteTo.Console()
                .WriteTo.File(
                    "logs/ex_.log",
                    LogEventLevel.Error,
                    "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] ({SourceContext}) [{CorrelationId}] {Message:lj}{NewLine}{Exception}",
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 10,
                    rollOnFileSizeLimit: true,
                    fileSizeLimitBytes: 10485760,
                    shared: true
                )
                .WriteTo.File(
                    "logs/dbg_.log",
                    LogEventLevel.Debug,
                    "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] ({SourceContext}) [{CorrelationId}] {Message:lj}",
                     rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 10,
                    rollOnFileSizeLimit: true,
                    fileSizeLimitBytes: 10485760,
                    shared: true
                )
                .CreateLogger();

            builder.UseSerilog(Log.Logger);
            return builder;
        }

        public static IServiceCollection AddSecurity(this IServiceCollection services, IConfiguration configuration)
        {
            //TODO Add policies
            services.AddAuthorization();
            services.AddJwtAuthenticationConfiguration(configuration);
            return services;
        }
    }
}
