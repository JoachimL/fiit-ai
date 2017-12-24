using Bodybuildr.Domain.Workouts;
using System;

namespace StrongR.ReadStack.WorkoutDetail
{
    public class ActivityDetail
    {
        public static ActivityDetail Empty { get; } = new ActivityDetail
        {
            Id = Guid.Empty,
            Rating = 1,
            Sets = new[] { new Set(), new Set(), new Set() }
        };

        public Guid Id { get; set; }
        public int Rating { get; set; }
        public Set[] Sets { get; set; }
        public string ExerciseId { get; set; }
    }
}