using System;
using System.Collections.Generic;
using System.Text;

namespace Bodybuildr.Domain.Workouts.Commands
{
    public class UpdateStartDateTime : Command
    {
        public UpdateStartDateTime(string userId, Guid workoutId, DateTimeOffset startDateTime, int version)
        {
            UserId = userId;
            WorkoutId = workoutId;
            StartDateTime = startDateTime;
            Version = version;
        }

        public string UserId { get; }
        public Guid WorkoutId { get; }
        public DateTimeOffset StartDateTime { get; }
        public int Version { get; }
    }
}
