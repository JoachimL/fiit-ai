using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using StrongR.ReadStack.EntityPropertyConverter;
using System;
using System.Collections.Generic;

namespace StrongR.ReadStack.Workouts.TableStorage
{
    public class Workout : TableEntity
    {
        public Workout()
        {

        }

        public Workout(string userId, Guid workoutId)
            : base(userId, workoutId.ToString("D"))
        {

        }

        public DateTimeOffset StartDateTime { get; set; }

        public int ActivityCount { get; set; }

        [EntityPropertyConverter]
        public WorkoutActivity[] Activities { get; set; } = new WorkoutActivity[0];
        public int Version { get; set; }

        public override IDictionary<string, EntityProperty> WriteEntity(OperationContext operationContext)
        {
            var results = base.WriteEntity(operationContext);
            EntityPropertyConvert.Serialize(this, results);
            return results;
        }

        public override void ReadEntity(IDictionary<string, EntityProperty> properties, OperationContext operationContext)
        {
            base.ReadEntity(properties, operationContext);
            EntityPropertyConvert.DeSerialize(this, properties);
        }
    }
}
