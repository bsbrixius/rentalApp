using Core.API.Domain.Events.Motorcycle;
using MediatR;

namespace Core.API.Application.DomainEvents.Handling
{
    public class MotorcycleDomainEventHandler :
        INotificationHandler<MotorcycleRegisteredDomainEvent>

    {
        private readonly ILogger<MotorcycleDomainEventHandler> _logger;

        public MotorcycleDomainEventHandler(
            ILogger<MotorcycleDomainEventHandler> logger
            )
        {
            _logger = logger;
        }
        public async Task Handle(MotorcycleRegisteredDomainEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Motorcycle registered: {0}", notification.Motorcycle);
        }
    }
}
