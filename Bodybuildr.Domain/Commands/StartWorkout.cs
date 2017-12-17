using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bodybuildr.Domain.Commands
{
    public class StartWorkout : IRequest
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
