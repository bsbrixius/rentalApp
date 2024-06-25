using Crosscutting.EventBus.RabbitMq;
using Crosscutting.EventStore.Postgres;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEventStoreForPostgres();
builder.Services.AddRabbitMqBroker();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<EventStoreContext>();
        dbContext.Database.EnsureDeleted();
        dbContext.Database.EnsureCreated();
    }
}

app.Run();
