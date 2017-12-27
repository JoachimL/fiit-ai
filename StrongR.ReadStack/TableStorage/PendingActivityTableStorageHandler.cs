using Microsoft.WindowsAzure.Storage.Table;
using StrongR.ReadStack.Workouts.TableStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StrongR.ReadStack.TableStorage
{
    public class PendingActivityTableStorageHandler
        : TableHandler<PendingActivity>
    {
        public PendingActivityTableStorageHandler(CloudTableClient cloudTableClient) : base(cloudTableClient)
        {
        }

        public override string TableName => "PendingActivities";

        public async Task StoreActivities(Guid workoutId, IEnumerable<Bodybuildr.Domain.Workouts.Activity> activities)
        {
            await EnsureTableExists();
            var table = Table;
            var operation = new TableBatchOperation();
            foreach (var x in activities.Select((a, idx) => new { Activity = a, Index = idx }))
                operation.InsertOrReplace(new PendingActivity(workoutId, x.Activity.Id)
                {
                    ExerciseId = x.Activity.ExerciseId,
                    Order = x.Index,
                    Sets = x.Activity.Sets,
                    ExerciseName = x.Activity.ExerciseName
                });
            await table.ExecuteBatchAsync(operation);
        }

        public Task<IEnumerable<PendingActivity>> GetActivities(Guid workoutId)
        {
            var query = new TableQuery<PendingActivity>();
            query.FilterString = TableQuery.GenerateFilterCondition(
                nameof(PendingActivity.PartitionKey),
                QueryComparisons.Equal,
                workoutId.ToString());
            return ExecuteQueryAsync(query);
        }
    }
}
