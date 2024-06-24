using Authentication.API;
using Authentication.API.Application.Queries.User;
using Authentication.API.Infraestructure;
using BuildingBlocks.API.Core;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var AppName = typeof(Program).Assembly.FullName;
builder.Host.AddSerilogCore();
Log.Information("Starting web host");
Log.Information(builder.Environment.EnvironmentName);
Log.Information("Configuring web host ({ApplicationContext})...", AppName);

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

builder.Services.AddScoped<IUserQueries, UserQueries>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<AuthenticationContext>();
        dbContext.Database.EnsureDeleted();
        dbContext.Database.EnsureCreated();
        await dbContext.TrySeedDatabaseAsync();
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
app.UseAuthentication();
app.UseRouting();
//app.ConfigureExceptionHandler(logger, env);
//app.UseMiddleware<JwtMiddleware>();
app.UseAuthorization();
//app.UseHealthChecks("/health", GetHealthCheckOptions());
app.MapControllers();

app.Run();
