using MediatR;

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
        public DateTime CreatedAt { get; protected set; }
        public DateTime? UpdatedAt { get; protected set; }

        private List<INotification> _domainEvents;
        public IReadOnlyCollection<INotification> DomainEvents => _domainEvents?.AsReadOnly();
        protected Entity()
        {
            CreatedAt = DateTime.UtcNow;
        }


        public void AddDomainEvent(INotification eventItem)
        {
            _domainEvents = _domainEvents ?? new List<INotification>();
            _domainEvents.Add(eventItem);
        }

        public void RemoveDomainEvent(INotification eventItem)
        {
            _domainEvents?.Remove(eventItem);
        }

        public void ClearDomainEvents()
        {
            _domainEvents?.Clear();
        }
    }
}
