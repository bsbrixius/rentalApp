using Crosscutting.EventStore.Postgres.Model;
using Microsoft.EntityFrameworkCore;

namespace Crosscutting.EventStore.Postgres.Service
{
    public interface IEventStoreService
    {
        Task AddEventAsync(EventData @event);
    }

    public class EventStoreService : IEventStoreService
    {
        private readonly DbSet<EventData> _events;
        private readonly EventStoreContext _eventStore;

        public EventStoreService(EventStoreContext eventStore)
        {
            _events = eventStore.Set<EventData>();
            _eventStore = eventStore;
        }
        public async Task AddEventAsync(EventData @event)
        {
            await _events.AddAsync(@event);
            await _eventStore.SaveChangesAsync();
        }
    }
}
