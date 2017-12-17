using Bodybuildr.CommandStack.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bodybuildr.Domain
{
    public interface IEventStore
    {
        Task SaveEvents(Guid aggregateId, IEnumerable<Event> events, int expectedVersion);

        Task<List<Event>> GetEventsForAggregate(Guid aggregateId);
    }
}
