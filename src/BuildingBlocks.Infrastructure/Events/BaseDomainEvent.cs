using MediatR;

namespace BuildingBlocks.Infrastructure.Events
{
    public abstract class BaseDomainEvent : INotification
    {
        public DateTimeOffset Timestamp { get; protected set; } = DateTimeOffset.UtcNow;
    }
}
