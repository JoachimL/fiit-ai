using System;

namespace StrongR.ReadStack.Workouts.TableStorage
{
    public class WorkoutActivity
    {
        public Guid ActivityId { get; set; }
        public string ExerciseName { get; set; }
        public DateTimeOffset Added { get; set; }
        public string ExerciseId { get; set; }
        public int SetCount { get; set; }
    }
}