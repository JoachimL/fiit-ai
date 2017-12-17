using Bodybuildr.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuildr.EventStore
{
    public class Repository<T> : IRepository<T> where T : AggregateRoot, new() //shortcut you can do as you see fit with new()
    {
        private readonly IEventStore _storage;

        public Repository(IEventStore storage)
        {
            _storage = storage;
        }

        public Task SaveAsync(AggregateRoot aggregate, int expectedVersion)
        {
            return _storage.SaveEvents(aggregate.Id, aggregate.GetUncommittedChanges().ToArray(), expectedVersion);
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            var obj = new T();//lots of ways to do this
            var e = await _storage.GetEventsForAggregate(id);
            obj.LoadsFromHistory(e);
            return obj;
        }
    }
}
