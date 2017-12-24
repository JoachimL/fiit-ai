using Bodybuildr.Domain.Workouts.Events;
using MediatR;
using StrongR.ReadStack.TableStorage;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StrongR.ReadStack.EventHandlers
{
    public class LastActivityForUserEventHandler : INotificationHandler<ActivityCompleted>
    {
        private readonly LastExerciseActivityTableHandler lastExerciseActivityTableHandler;
        private readonly WorkoutTableHandler _workoutTableHandler;
        public LastActivityForUserEventHandler(
            LastExerciseActivityTableHandler lastExerciseActivityTableHandler,
            WorkoutTableHandler workoutTableHandler)
        {
            this.lastExerciseActivityTableHandler = lastExerciseActivityTableHandler;
            _workoutTableHandler = workoutTableHandler;
        }

        public async Task Handle(ActivityCompleted notification, CancellationToken cancellationToken)
        {
            var workout = await _workoutTableHandler.GetWorkout(notification.UserId, notification.WorkoutId);
            if (workout != null)
            {
                var last = await lastExerciseActivityTableHandler.Retrieve(notification.UserId, notification.ExerciseId);
                if (last == null || workout.StartDateTime >= last.ExerciseDateTime)
                {
                    await lastExerciseActivityTableHandler.InsertOrReplace(new Workouts.TableStorage.LastExerciseActivity(notification.UserId, notification.ExerciseId)
                    {
                        ExerciseDateTime = workout.StartDateTime,
                        Sets = notification.Sets.ToArray()
                    });
                }
            }
        }
    }
}
