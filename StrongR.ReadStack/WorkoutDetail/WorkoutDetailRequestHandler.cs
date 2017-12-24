using Bodybuildr.Domain.Workouts;
using MediatR;
using Microsoft.WindowsAzure.Storage.Table;
using StrongR.ReadStack.TableStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StrongR.ReadStack.WorkoutDetail
{
    public class WorkoutDetailRequestHandler : IRequestHandler<WorkoutDetailRequest, WorkoutDetailResponse>
    {
        private readonly WorkoutTableHandler _tableHandler;
        private readonly ActivityTableHandler _activitytableHandler;
        private readonly PendingActivityTableStorageHandler _pendingActivityTableStorageHandler;

        public WorkoutDetailRequestHandler(
            WorkoutTableHandler tableHandler, 
            ActivityTableHandler activitytableHandler, 
            PendingActivityTableStorageHandler pendingActivityTableStorageHandler
            )
        {
            _tableHandler = tableHandler;
            _activitytableHandler = activitytableHandler;
            _pendingActivityTableStorageHandler = pendingActivityTableStorageHandler;
        }

        public async Task<WorkoutDetailResponse> Handle(WorkoutDetailRequest request, CancellationToken cancellationToken)
        {
            var workout = await _tableHandler.GetWorkout(request.UserId, request.WorkoutId);
            return new WorkoutDetailResponse
            {
                Activities = GetActivities(workout),
                PendingActivities = await GetPendingActivitiesAsync(request.WorkoutId),
                StartedDateTime = workout.StartDateTime,
                Id = request.WorkoutId,
                SelectedActivity = await GetActivityFor(request),
                Version = workout.Version,
            };
        }

        private async Task<ActivityDetail> GetActivityFor(WorkoutDetailRequest request)
        {
            return request.ActivityId == Guid.Empty ? ActivityDetail.Empty : await GetActivityAsync(request.WorkoutId, request.ActivityId);
        }

        private async Task<ActivityDetail> GetActivityAsync(Guid workoutId, Guid activityId)
        {
            var activity = await _activitytableHandler.Retrieve(workoutId, activityId);
            if (activity == null)
                return ActivityDetail.Empty;
            return new ActivityDetail
            {
                Id = activity.ActivityId,
                Rating = activity.Rating,
                Sets = activity.Sets.Select(s => new Set { Repetitions = s.Repetitions, SystemOfMeasurement = Bodybuildr.SystemOfMeasurement.Metric, Weight = s.Weight }).ToArray(),
                ExerciseId = activity.ExerciseId
            };
        }

        private async Task<ActivityListDetail[]> GetPendingActivitiesAsync(Guid workoutId)
        {
            var pending = await _pendingActivityTableStorageHandler.GetActivities(workoutId);
            return pending?.Select(a => new ActivityListDetail
            {
                ExerciseId = a.ExerciseId,
                ExerciseName = a.ExerciseName,
                Id = a.ActivityId
            }).ToArray() ?? new ActivityListDetail[0];
        }

        private static ActivityListDetail[] GetActivities(Workouts.TableStorage.Workout workout)
        {
            return workout.Activities?.Select(a => new ActivityListDetail
            {
                ExerciseId = a.ExerciseId,
                ExerciseName = a.ExerciseName,
                Id = a.ActivityId
            }).ToArray() ?? new ActivityListDetail[0];
        }
    }
}
