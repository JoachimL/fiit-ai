using System;
using System.Collections.Generic;
using System.Text;

namespace Bodybuildr.Domain.Workouts.Commands
{
    public class UpdateStartDateTime : Command
    {
        public UpdateStartDateTime(Guid workoutId, DateTimeOffset startDateTime, int version)
        {
            WorkoutId = workoutId;
            StartDateTime = startDateTime;
            Version = version;
        }

        public Guid WorkoutId { get; }
        public DateTimeOffset StartDateTime { get; }
        public int Version { get; }
    }
}
