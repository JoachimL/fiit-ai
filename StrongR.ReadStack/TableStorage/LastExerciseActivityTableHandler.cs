using Microsoft.WindowsAzure.Storage.Table;
using StrongR.ReadStack.Workouts.TableStorage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StrongR.ReadStack.TableStorage
{
    public class LastExerciseActivityTableHandler : TableHandler<LastExerciseActivity>
    {
        public LastExerciseActivityTableHandler(CloudTableClient tableClient) : base(tableClient)
        {

        }

        public override string TableName => "LastActivityWithExercise";

        public new Task<LastExerciseActivity> Retrieve(string userId, string exerciseId)
        {
            return base.Retrieve(userId, exerciseId);
        }
    }
}
