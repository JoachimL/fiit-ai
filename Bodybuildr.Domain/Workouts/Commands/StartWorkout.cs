using MediatR;
using System;

namespace Bodybuildr.Domain.Workouts.Commands
{
    public class StartWorkout : Command, IRequest
    {
        public StartWorkout(
            Guid workoutId, 
            string userId, 
            DateTimeOffset startDateTime)
        {
            WorkoutId = workoutId;
            StartDateTime = startDateTime;
            UserId = userId;
        }

        public Guid WorkoutId { get; }
        public DateTimeOffset StartDateTime { get; }
        public string UserId { get; }
    }
}
