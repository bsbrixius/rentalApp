using Core.Application.IntegrationEvents.Events.Motorcycle;
using Crosscutting.EventStore.Postgres.Model;
using Crosscutting.EventStore.Postgres.Service;
using Crosscutting.Mail;
using MassTransit;
using System.Text.Json;

namespace Worker.Api.Application.Worker.BusConsumer
{
    public class MotorcycleEventConsumer : IConsumer<MotorcycleRegisteredIntegrationEvent>
    {
        private readonly IEventStoreService _eventStoreService;
        private readonly IMailService _mailService;

        public MotorcycleEventConsumer(
            IEventStoreService eventStoreService,
            IMailService mailService)
        {
            _eventStoreService = eventStoreService;
            _mailService = mailService;
        }
        public async Task Consume(ConsumeContext<MotorcycleRegisteredIntegrationEvent> context)
        {
            var motorcycleRegisteredIntegrationEvent = context.Message;
            if (motorcycleRegisteredIntegrationEvent.Year > 2023)
            {
                await _mailService.SendEmailAsync("User", "userEmail@rentalapp.com", "Motorcycle Registration", $"The motorcycle registration year is greater than 2024. {JsonSerializer.Serialize(motorcycleRegisteredIntegrationEvent)}");
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
