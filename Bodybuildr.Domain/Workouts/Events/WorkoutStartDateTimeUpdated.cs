using System;

namespace Bodybuildr.Domain.Workouts.Events
{
    public class WorkoutStartDateTimeUpdated : Event
    {
        public WorkoutStartDateTimeUpdated(
            string userId,
            Guid workoutId,
            DateTimeOffset startDateTime)
        {
            UserId = userId;
            WorkoutId = workoutId;
            StartDateTime = startDateTime;
        }

        public DateTimeOffset StartDateTime { get; }
        public Guid WorkoutId { get; set; }
        public string UserId { get; set; }
    }
}
