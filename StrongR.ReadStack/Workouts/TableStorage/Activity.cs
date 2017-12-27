using Bodybuildr.Domain.Workouts;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using StrongR.ReadStack.EntityPropertyConverter;
using System;
using System.Collections.Generic;
using System.Text;

namespace StrongR.ReadStack.Workouts.TableStorage
{
    public class Activity : ComplexTableEntity
    {
        public Activity() { }

        public Activity(Guid workoutId, Guid activityId)
            : base(workoutId.ToString("D"), activityId.ToString("D"))
        {

        }

        [IgnoreProperty]
        public Guid WorkoutId => Guid.Parse(PartitionKey);

        public DateTimeOffset WorkoutStarted { get; set; }

        [IgnoreProperty]
        public Guid ActivityId => Guid.Parse(RowKey);

        public string ExerciseId { get; set; }
        public DateTimeOffset Added { get; set; }
        public string UserId { get; set; }

        public int Rating { get; set; }

        [EntityPropertyConverter]
        public Set[] Sets { get; set; } = new Set[] { new Set(), new Set(), new Set() };


    }
}
