using System;
using System.Collections.Generic;
using System.Text;

namespace Bodybuildr.Domain.Workouts.Events
{
    public class WorkoutDeleted : Event
    {
        public Guid WorkoutId { get; }
        public string UserId { get; set; }

        public WorkoutDeleted(Guid workoutId, string userId)
        {
            WorkoutId = workoutId;
            UserId = userId;
        }
    }
}
