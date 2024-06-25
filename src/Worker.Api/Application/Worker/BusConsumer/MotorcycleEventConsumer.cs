using Core.Application.IntegrationEvents.Events.Motorcycle;
using Crosscutting.EventStore.Postgres.Model;
using Crosscutting.EventStore.Postgres.Service;
using MassTransit;
using System.Text.Json;

namespace Worker.Api.Application.Worker.BusConsumer
{
    public class MotorcycleEventConsumer : IConsumer<MotorcycleRegisteredIntegrationEvent>
    {
        private readonly IEventStoreService _eventStoreService;

        public MotorcycleEventConsumer(IEventStoreService eventStoreService)
        {
            _eventStoreService = eventStoreService;
        }
        public async Task Consume(ConsumeContext<MotorcycleRegisteredIntegrationEvent> context)
        {
            var motorcycleRegisteredIntegrationEvent = context.Message;
            if (motorcycleRegisteredIntegrationEvent.Year > 2023)
            {
                //TODO notify
            }
            await _eventStoreService.AddEventAsync(new EventData
            {
                AggregateId = motorcycleRegisteredIntegrationEvent.Id,
                EventId = motorcycleRegisteredIntegrationEvent.EventId,
                Data = JsonSerializer.Serialize(motorcycleRegisteredIntegrationEvent),
                Type = motorcycleRegisteredIntegrationEvent.GetType().Name
            });

        }
    }
}
