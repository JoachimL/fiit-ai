using Bodybuildr.Domain.Workouts.Events;
using MediatR;
using StrongR.ReadStack.TableStorage;
using StrongR.ReadStack.Workouts.TableStorage;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StrongR.ReadStack.EventHandlers
{
    public class WorkoutTableEventHandler :
        INotificationHandler<WorkoutCreated>,
        INotificationHandler<ActivityCompleted>,
        INotificationHandler<WorkoutStartDateTimeUpdated>
    {
        private readonly WorkoutTableHandler _workoutTableHandler;
        private readonly ActivityTableHandler _activityTableHandler;
        private readonly ExerciseRepository _exerciseRepository;

        public WorkoutTableEventHandler(
            WorkoutTableHandler workoutTableHandler,
            ActivityTableHandler activityTableHandler,
            ExerciseRepository exerciseRepository)
        {
            _workoutTableHandler = workoutTableHandler;
            _activityTableHandler = activityTableHandler;
            _exerciseRepository = exerciseRepository;
        }
        public async Task Handle(WorkoutCreated notification, CancellationToken cancellationToken)
        {
            var workout = new Workout(notification.UserId, notification.Id);
            workout.StartDateTime = notification.StartDateTime;
            workout.Version = notification.Version;
            await _workoutTableHandler.InsertOrReplace(workout);
        }

        public async Task Handle(ActivityCompleted notification, CancellationToken cancellationToken)
        {
            Workout workout = await GetWorkoutAsync(notification.UserId, notification.WorkoutId);
            var activity = new Activity(notification.WorkoutId, notification.ActivityId)
            {
                WorkoutStarted = workout.StartDateTime,
                ExerciseId = notification.ExerciseId,
                Added = notification.Added,
                UserId = notification.UserId,
                Sets = notification.Sets?.ToArray(),
                Rating = notification.Rating
            };
            await _activityTableHandler.InsertOrReplace(activity);

            var exercises = await _exerciseRepository.GetAllExercisesAsync();
            
            var workoutActivity = new WorkoutActivity
            {
                ActivityId = notification.ActivityId,
                Added = notification.Added,
                ExerciseId = notification.ExerciseId,
                SetCount = notification.Sets.Count(),
                ExerciseName = exercises.SingleOrDefault(s => s.Id == notification.ExerciseId).Name,
            };
            var activities = workout.Activities.ToList();
            var existingIndex = activities.Select((x, idx) => new { x.ActivityId, idx })
                .Where(p => p.ActivityId == notification.ActivityId)
                .Select(p => p.idx + 1)
                .FirstOrDefault() - 1;
            if (existingIndex == -1)
                activities.Add(workoutActivity);
            else
                activities[existingIndex] = workoutActivity;
            workout.Activities = activities.ToArray();
            workout.ActivityCount = workout.Activities.Count();
            workout.Version = notification.Version;
            await _workoutTableHandler.InsertOrReplace(workout);
 
        }

        private async Task<Workout> GetWorkoutAsync(string userId, Guid workoutId)
        {
            return await _workoutTableHandler.GetWorkout(
                            userId,
                            workoutId);
        }

        public async Task Handle(WorkoutStartDateTimeUpdated notification, CancellationToken cancellationToken)
        {
            var workout = await GetWorkoutAsync(notification.UserId, notification.WorkoutId);
            workout.StartDateTime = notification.StartDateTime;
            workout.Version = notification.Version;
            await _workoutTableHandler.InsertOrReplace(workout);
        }
    }
}
