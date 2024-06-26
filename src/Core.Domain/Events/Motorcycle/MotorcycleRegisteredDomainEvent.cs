
using BuildingBlocks.Domain.Events;

namespace Core.Domain.Events.Motorcycle
{
    public class MotorcycleRegisteredDomainEvent : DomainEvent
    {
        public MotorcycleRegisteredDomainEvent(Domain.Aggregates.Motorcycle.Motorcycle motorcycle)
        {
            Motorcycle = motorcycle;
        }

        public Aggregates.Motorcycle.Motorcycle Motorcycle { get; private set; }
    }
}
