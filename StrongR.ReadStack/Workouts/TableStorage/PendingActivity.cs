using Bodybuildr.Domain.Workouts;
using Microsoft.WindowsAzure.Storage.Table;
using StrongR.ReadStack.EntityPropertyConverter;
using System;

namespace StrongR.ReadStack.Workouts.TableStorage
{
    public class PendingActivity : ComplexTableEntity
    {
        public PendingActivity() { }

        public PendingActivity(Guid workoutId, Guid activityId)
            : base(workoutId.ToString("D"), activityId.ToString("D"))
        {

        }

        public int Order { get; set; }

        [IgnoreProperty]
        public Guid WorkoutId => Guid.Parse(PartitionKey);
        
        [IgnoreProperty]
        public Guid ActivityId => Guid.Parse(RowKey);

        public string ExerciseId { get; set; }

        public string ExerciseName { get; set; }

        [EntityPropertyConverter]
        public Set[] Sets { get; set; }

      
    }
}
