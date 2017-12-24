using System;
using System.Collections.Generic;
using System.Text;

namespace Bodybuildr.Domain.Workouts.Events
{
    public class ActivitiesCopiedFromWorkout : Event
    {
        public string UserId { get; set; }
        public Guid WorkoutId { get; }
        public Guid WorkoutToCopyId { get; }
        public IEnumerable<Activity> Activities { get; }

        public ActivitiesCopiedFromWorkout(Guid workoutId, string userId, Guid workoutToCopyId, IEnumerable<Activity> activities)
        {
            WorkoutId = workoutId;
            UserId = userId;
            WorkoutToCopyId = workoutToCopyId;
            Activities = activities;
        }
    }
}
