using Autofac.Extensions.DependencyInjection;
using BuildingBlocks.API.Core;
using Core.Infrastructure;
using Serilog;


var builder = WebApplication.CreateBuilder(args);
var AppName = typeof(Program).Assembly.FullName;

builder.Host.AddSerilogCore();

Log.Information("Starting web host");
Log.Information(builder.Environment.EnvironmentName);

Log.Information("Configuring web host ({ApplicationContext})...", AppName);
// Add services to the container.
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());


builder.Services.AddDbContext<RentalAppContext>();

//builder.Services.AddRabbitMqBroker();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    Log.Information("Applying migrations ({ApplicationContext})...", AppName);
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<RentalAppContext>();
        var listaDemoto = dbContext.Motorcycles.ToList();
        //dbContext.Database.EnsureDeleted();
        //dbContext.Database.EnsureCreated();

        //dbContext.TryInitializeDatabaseTables(app.Configuration);
        //dbContext.TrySeedDatabase(app.Configuration);
    }
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
