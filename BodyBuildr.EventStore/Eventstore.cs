using Bodybuildr.Domain;
using Bodybuildr.Domain.Workouts.Events;
using MediatR;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using Streamstone;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace BodyBuildr.EventStore
{
    public class EventStore : IEventStore
    {
        private readonly CloudTable _table;
        private readonly IMediator _mediator;

        public EventStore(CloudTable table, IMediator mediator)
        {
            _mediator = mediator;
            _table = table;
        }

        public async Task SaveEvents(Guid aggregateId, IEnumerable<Event> events, int expectedVersion)
        {
            var i = expectedVersion;

            // iterate through current aggregate events increasing version with each processed event
            foreach (var @event in events)
            {
                i++;
                @event.Version = i;
            }

            var paritionKey = aggregateId.ToString("D");
            var partition = new Partition(_table, paritionKey);

            var existent = await Stream.TryOpenAsync(partition);
            var stream = existent.Found
                ? existent.Stream
                : new Stream(partition);

            if (stream.Version != expectedVersion && expectedVersion > -1)
                throw new ConcurrencyException();

            try
            {
                await Stream.WriteAsync(stream, events.Select(ToEventData).ToArray());
            }
            catch (ConcurrencyConflictException e)
            {
                throw new ConcurrencyException();
            }

            foreach (var @event in events)
            {
                // publish current event to the bus for further processing by subscribers
                await _mediator.Publish(@event);
            }
        }

        // collect all processed events for given aggregate and return them as a list
        // used to build up an aggregate from its history (Domain.LoadsFromHistory)
        public async Task<List<Event>> GetEventsForAggregate(Guid aggregateId)
        {
            var paritionKey = aggregateId.ToString("D");
            var partition = new Partition(_table, paritionKey);

            if (!(await Stream.ExistsAsync(partition)))
            {
                throw new AggregateNotFoundException();
            }

            return (await Stream.ReadAsync<EventEntity>(partition)).Events.Select(ToEvent).ToList();
        }

        static Event ToEvent(EventEntity e)
        {
            var t = Type.GetType(e.Type);
            if (t == null)
            {
                Assembly a = Assembly.GetAssembly(typeof(WorkoutCreated));
                t = a.GetType(e.Type);
            }
            return (Event)JsonConvert.DeserializeObject(e.Data, t);
        }

        static EventData ToEventData(Event e)
        {
            var id = Guid.NewGuid();

            var properties = new
            {
                Id = id,
                Type = e.GetType().FullName,
                Data = JsonConvert.SerializeObject(e)
            };

            return new EventData(EventId.From(id), EventProperties.From(properties));
        }

        class EventEntity : TableEntity
        {
            public string Type { get; set; }
            public string Data { get; set; }
        }
    }

    public class AggregateNotFoundException : Exception
    {
    }

    public class ConcurrencyException : Exception
    {
    }
}
