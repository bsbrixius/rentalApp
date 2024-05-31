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

        private List<INotification> _domainEvents;
        public IReadOnlyCollection<INotification> DomainEvents => _domainEvents?.AsReadOnly();
        protected Entity()
        {
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
