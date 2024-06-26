using MediatR;

namespace BuildingBlocks.Domain.Events
{
    public abstract class DomainEvent : INotification
    {
        public DomainEvent()
        {
            CorrelationId = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
        }
        public DateTime CreatedAt { get; private set; }
        public Guid CorrelationId { get; private set; }
    }
}
