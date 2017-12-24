using Bodybuildr.Domain.Workouts;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using StrongR.ReadStack.EntityPropertyConverter;
using System;
using System.Collections.Generic;
using System.Text;

namespace StrongR.ReadStack.Workouts.TableStorage
{
    public class LastExerciseActivity : TableEntity
    {
        public LastExerciseActivity()
        {

        }
        public LastExerciseActivity(string userId, string exerciseId)
          : base(userId, exerciseId)
        {

        }


        public DateTimeOffset ExerciseDateTime { get; set; }
        [EntityPropertyConverter]
        public Set[] Sets { get; set; }

        public int Rating { get; set; }

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
