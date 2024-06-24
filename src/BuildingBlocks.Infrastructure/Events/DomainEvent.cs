using MediatR;

namespace BuildingBlocks.EventSourcing
{
    public abstract class DomainEvent : INotification
    {
        public DomainEvent()
        {
            DomainNotificationId = Guid.NewGuid();
            Timestamp = DateTime.UtcNow;
        }
        public DateTime Timestamp { get; private set; }
        public Guid DomainNotificationId { get; private set; }
    }
}
