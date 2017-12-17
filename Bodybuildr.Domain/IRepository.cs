using System;
using System.Threading.Tasks;

namespace Bodybuildr.Domain
{
    public interface IRepository<T> where T : AggregateRoot, new()
    {
        Task SaveAsync(AggregateRoot aggregate, int expectedVersion);
        Task<T> GetByIdAsync(Guid id);
    }
}
