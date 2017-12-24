using MediatR;
using System;

namespace StrongR.ReadStack.WorkoutDetail
{
    public class WorkoutDetailRequest : IRequest<WorkoutDetailResponse>
    {
        public WorkoutDetailRequest(Guid workoutId, string userId, Guid activityId)
        {
            WorkoutId = workoutId;
            UserId = userId;
            ActivityId = activityId;
        }

        public Guid WorkoutId { get; }
        public string UserId { get; }
        public Guid ActivityId { get; }
    }
}
