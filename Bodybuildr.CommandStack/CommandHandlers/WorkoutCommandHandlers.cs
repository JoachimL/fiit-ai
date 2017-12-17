using Bodybuildr.Domain;
using Bodybuildr.Domain.Commands;
using Bodybuildr.Domain.Workouts;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Bodybuildr.CommandStack.CommandHandlers
{
    public class WorkoutCommandHandlers : IRequestHandler<StartWorkout>
    {
        private readonly IRepository<Workout> _repository;

        public WorkoutCommandHandlers(IRepository<Workout> repository)
        {
            _repository = repository;
        }

        public Task Handle(StartWorkout message, CancellationToken cancellationToken)
        {
            var newWorkout = new Workout(message.WorkoutId, message.UserId, message.StartDateTime);
            return _repository.SaveAsync(newWorkout, -1);
        }
    }
}
