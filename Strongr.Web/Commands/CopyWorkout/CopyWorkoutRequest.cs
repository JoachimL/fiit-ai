using MediatR;
using System;

namespace Strongr.Web.Commands.CopyWorkout
{
    public class CopyWorkoutRequest : IRequest<CopyWorkoutResponse>
    {
        public CopyWorkoutRequest(
            Guid workoutId, 
            Guid workoutToCopy,
            string userId, 
            DateTimeOffset startDateTime)
        {
            WorkoutId = workoutId;
            WorkoutToCopy = workoutToCopy;
            StartDateTime = startDateTime;
            UserId = userId;
        }

        public Guid WorkoutId { get; }
        public Guid WorkoutToCopy { get; }
        public DateTimeOffset StartDateTime { get; }
        public string UserId { get; }
    }
}
