using BuildingBlocks.EventSourcing;

namespace BuildingBlocks.Domain
{
    public interface IEntity
    {
        //IEntity ApplyEvent(Event payload);
        //List<Event> GetUncommittedEvents();
        //IEntity AddEvent(Event uncommittedEvent);
        //void ClearUncommittedEvents();
    }

    public abstract class Entity : IEntity
    {
        Guid _Id;
        public virtual Guid Id
        {
            get
            {
                return _Id;
            }
            protected set
            {
                _Id = value;
            }
        }

        private List<DomainEvent> _domainEvents = new List<DomainEvent>();
        public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();
        protected Entity()
        {
            _Id = Guid.NewGuid();
        }

        public void AddDomainEvent(DomainEvent eventItem)
        {
            _domainEvents = _domainEvents ?? new List<DomainEvent>();
            _domainEvents.Add(eventItem);
        }

        public void RemoveDomainEvent(DomainEvent eventItem)
        {
            _domainEvents?.Remove(eventItem);
        }

        public void ClearDomainEvents()
        {
            _domainEvents?.Clear();
        }
    }
}
