using Microsoft.WindowsAzure.Storage.Table;
using StrongR.ReadStack.Workouts.TableStorage;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StrongR.ReadStack.TableStorage
{
    public class ActivityTableHandler : TableHandler<Activity>
    {
        
        public ActivityTableHandler(CloudTableClient tableClient) : base(tableClient)
        {
        
        }

        public override string TableName => "Activities";

        public Task<Activity> Retrieve(Guid workoutId, Guid activityId)
        {
            return base.Retrieve(workoutId.ToString(), activityId.ToString());
        }

        public async Task<IEnumerable<Activity>> RetrieveActivitiesForWorkoutAsync(Guid workoutId)
        {
            var query = new TableQuery<Activity>();
            query.FilterString = TableQuery.GenerateFilterCondition(nameof(Activity.PartitionKey), QueryComparisons.Equal, workoutId.ToString());
            return await Table.ExecuteQueryAsync(query);
            
        }
    }
}
