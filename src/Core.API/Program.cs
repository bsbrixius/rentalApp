using BuildingBlocks.API.Core.Swagger;
using Core.API;
using Core.API.Infraestructure;
using Core.Data;
using Serilog;


var builder = WebApplication.CreateBuilder(args);

var AppName = typeof(Program).Assembly.GetName().Name;
builder.Host.UseSerilogCore();
Log.Information("Starting web host");
Log.Information(builder.Environment.EnvironmentName);
Log.Information("Configuring web host ({ApplicationContext})...", AppName);

var Configuration = builder.Configuration;
////Autofac Modules
builder.Host.UseAutofacIoC(Configuration);

builder.Services
    .AddWebAppConfiguration(Configuration)
    .AddHealthChecks(Configuration)
    .AddDbContext(Configuration)
    .AddServices(Configuration)
    .AddSecurity(Configuration)
    .AddSwagger(Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<CoreContext>();
        dbContext.Database.EnsureDeleted();
        dbContext.Database.EnsureCreated();
        await dbContext.TrySeedDevelopmentDatabaseAsync();
    }
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.ShowCommonExtensions();
        c.SwaggerEndpointByArea();
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
app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();
//app.UseHealthChecks("/health", GetHealthCheckOptions());
app.MapControllers();

app.Run();

public partial class Program { }