using Bodybuildr.Domain.Workouts;
using Bodybuildr.Domain.Workouts.Commands;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Bodybuildr.Domain.CommandHandlers
{
    public class WorkoutCommandHandlers : 
        IRequestHandler<StartWorkout>,
        IRequestHandler<CompleteActivity, CompleteActivityResponse>,
        IRequestHandler<UpdateStartDateTime>,
        IRequestHandler<CopyActivitiesFromWorkout>,
        IRequestHandler<DeleteWorkout>
    {
        private readonly IRepository<Workout> _repository;

        public WorkoutCommandHandlers(IRepository<Workout> repository)
        {
            _repository = repository;
        }

        public Task Handle(StartWorkout message, CancellationToken cancellationToken)
        {
            var newWorkout = new Workout(message.WorkoutId, message.UserId, message.StartDateTime);
            return _repository.SaveAsync(newWorkout, 0);
        }

        public async Task<CompleteActivityResponse> Handle(CompleteActivity request, CancellationToken cancellationToken)
        {
            var item = await _repository.GetByIdAsync(request.WorkoutId);
            var activityId = Guid.NewGuid();
            item.AddActivity(
                activityId,
                request.ExerciseId,
                request.Sets,
                request.Rating,
                request.Added);
            await _repository.SaveAsync(item, request.Version);
            return new CompleteActivityResponse { ActivityId = activityId };
        }

        public async Task Handle(UpdateStartDateTime message, CancellationToken cancellationToken)
        {
            var item = await _repository.GetByIdAsync(message.WorkoutId);
            item.UpdateStartTime(message.StartDateTime);
            await _repository.SaveAsync(item, message.Version);
        }

        public async Task Handle(CopyActivitiesFromWorkout message, CancellationToken cancellationToken)
        {
            var item = await _repository.GetByIdAsync(message.TargetWorkoutId);
            item.CopyActivitiesFromWorkout(message.WorkoutToCopy, message.Activities);
            await _repository.SaveAsync(item, message.Version);
        }

        public async Task Handle(DeleteWorkout message, CancellationToken cancellationToken)
        {
            var item = await _repository.GetByIdAsync(message.WorkoutId);
            item.Delete();
            await _repository.SaveAsync(item, message.OriginalVersion);
        }
    }
}
