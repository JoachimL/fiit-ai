using Bodybuildr.Domain.Workouts.Events;
using MediatR;
using StrongR.ReadStack.TableStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StrongR.ReadStack.EventHandlers
{
    public class ActivitiesCopiedFromWorkoutHandler
        : INotificationHandler<ActivitiesCopiedFromWorkout>
    {
        private readonly PendingActivityTableStorageHandler _activitiesCopiedFromWorkoutTableStorageHandler;
        private readonly WorkoutTableHandler workoutTableHandler;
        private readonly ExerciseRepository _exerciseRepository;

        public ActivitiesCopiedFromWorkoutHandler(
            PendingActivityTableStorageHandler activitiesCopiedFromWorkoutTableStorageHandler,
            WorkoutTableHandler workoutTableHandler, 
            ExerciseRepository exerciseRepository)
        {
            this._activitiesCopiedFromWorkoutTableStorageHandler = activitiesCopiedFromWorkoutTableStorageHandler;
            this.workoutTableHandler = workoutTableHandler;
            _exerciseRepository = exerciseRepository;
        }

        public async Task Handle(ActivitiesCopiedFromWorkout notification, CancellationToken cancellationToken)
        {
            var allExercises = await _exerciseRepository.GetAllExercisesAsync();
            await _activitiesCopiedFromWorkoutTableStorageHandler
                .StoreActivities(
                   notification.WorkoutId,
                    notification.Activities.Select(a => new Bodybuildr.Domain.Workouts.Activity
                    {
                        ExerciseId = a.ExerciseId,
                        ExerciseName = allExercises.SingleOrDefault(e=>a.ExerciseId==e.Id)?.Name ?? string.Empty,
                        Id = a.Id,
                        Sets = a.Sets
                    }));
        }
    }
}
