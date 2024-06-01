using Authentication.API;
using Authentication.API.Infraestructure;

var builder = WebApplication.CreateBuilder(args);

//var AppName = typeof(Program).Assembly.FullName;
//builder.Host.AddSerilogCore();
//Log.Information("Starting web host");
//Log.Information(builder.Environment.EnvironmentName);
//Log.Information("Configuring web host ({ApplicationContext})...", AppName);
////Autofac Modules
//builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
var Configuration = builder.Configuration;

builder.Services
    .AddIoC(Configuration)
    .AddWebAppConfiguration(Configuration)
    .AddHealthChecks(Configuration)
    .AddDbContext(Configuration)
    .AddSwagger(Configuration)
    .AddServices(Configuration)
    .AddSecurity(Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //Log.Information("Applying migrations ({ApplicationContext})...", AppName);
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<AuthenticationContext>();
        dbContext.Database.EnsureDeleted();
        dbContext.Database.EnsureCreated();
        await dbContext.TrySeedDatabaseAsync();
    }

    app.UseSwagger();
    app.UseReDoc(c =>
    {
        c.RoutePrefix = "swagger";
    });
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
//app.MapIdentityApi<User>();

app.Run();
