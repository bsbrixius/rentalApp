
using BuildingBlocks.EventSourcing;

namespace Driver.Domain.Events.Motorcycle
{
    public class MotorcycleRegisteredDomainEvent : DomainEvent
    {
        public MotorcycleRegisteredDomainEvent(Domain.Motorcycle motorcycle)
        {
            Motorcycle = motorcycle;
        }

        public Domain.Motorcycle Motorcycle { get; private set; }
    }
}
