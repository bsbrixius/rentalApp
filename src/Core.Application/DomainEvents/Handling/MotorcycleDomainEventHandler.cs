using Core.Application.IntegrationEvents.Events.Motorcycle;
using Core.Domain.Events.Motorcycle;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Core.Application.DomainEvents.Handling
{
    public class MotorcycleDomainEventHandler :
        INotificationHandler<MotorcycleRegisteredDomainEvent>

    {
        private readonly ILogger<MotorcycleDomainEventHandler> _logger;
        private readonly IBus _bus;

        public MotorcycleDomainEventHandler(
            ILogger<MotorcycleDomainEventHandler> logger,
            IBus bus
            )
        {
            _logger = logger;
            _bus = bus;
        }
        public async Task Handle(MotorcycleRegisteredDomainEvent notification, CancellationToken cancellationToken)
        {
            var integrationEvent = new MotorcycleRegisteredIntegrationEvent
            {
                CorrelationId = notification.CorrelationId,
                Id = notification.Motorcycle.Id,
                Year = notification.Motorcycle.Year,
                Model = notification.Motorcycle.Model,
                Plate = notification.Motorcycle.Plate,
            };
            _logger.LogInformation("Motorcycle registered: {motorcycle}", notification.Motorcycle);
            await _bus.Publish(integrationEvent);
        }
    }
}
