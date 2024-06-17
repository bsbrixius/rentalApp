using Authentication.API;
using Authentication.API.Infraestructure;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

//var AppName = typeof(Program).Assembly.FullName;
//builder.Host.AddSerilogCore();
//Log.Information("Starting web host");
//Log.Information(builder.Environment.EnvironmentName);
//Log.Information("Configuring web host ({ApplicationContext})...", AppName);

////Autofac Modules
builder.Host.UseAutofacIoC();


var Configuration = builder.Configuration;
builder.Services
    .AddWebAppConfiguration(Configuration)
    .AddHealthChecks(Configuration)
    .AddDbContext(Configuration)
    .AddServices(Configuration)
    .AddSecurity(Configuration)
    .AddSwagger(Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var teste0 = scope.ServiceProvider.GetRequiredService<IMediator>();
        var teste1 = scope.ServiceProvider.GetRequiredService<ILogger<AuthenticationContext>>();
        var teste2 = scope.ServiceProvider.GetRequiredService<IHttpContextAccessor>();
        var teste3 = scope.ServiceProvider.GetRequiredService<IConfiguration>();
        //scope.ServiceProvider.GetRequiredService<IUserRepository<User>>();
        //scope.ServiceProvider.GetRequiredService<IUserClaimRepository<User>>();
        //scope.ServiceProvider.GetRequiredService<IUserService<User>>();
        //var teste4 = scope.ServiceProvider.GetRequiredService<DbContextOptions<AuthenticationBaseContext<User>>>();


        var dbContext = scope.ServiceProvider.GetRequiredService<AuthenticationContext>();
        dbContext.Database.EnsureDeleted();
        dbContext.Database.EnsureCreated();
        await dbContext.TrySeedDatabaseAsync(scope.ServiceProvider);
    }
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// global cors policy
app.UseCors(x => x
    .SetPreflightMaxAge(TimeSpan.FromDays(7))
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true) // allow any origin
    .AllowCredentials()); // allow credentials

app.UseResponseCaching();

app.UseRouting();

//app.ConfigureExceptionHandler(logger, env);

app.UseAuthentication();
app.UseAuthorization();

//app.UseHealthChecks("/health", GetHealthCheckOptions());

app.MapControllers();


//TODO utilizar?
//app.MapGroup("api/auth").MapIdentityApi<User>();

app.Run();
