using Authentication.API;
using Authentication.API.Infraestructure;

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
