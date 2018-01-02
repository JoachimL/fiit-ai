using Microsoft.WindowsAzure.Storage.Table;
using StrongR.ReadStack.Workouts.TableStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StrongR.ReadStack.TableStorage
{
    public class WorkoutTableHandler : TableHandler<Workout>
    {

        public WorkoutTableHandler(CloudTableClient tableClient) : base(tableClient)
        {

        }

        public const string WorkoutTableName = "Workouts";
        public override string TableName => WorkoutTableName;

        public Task<Workout> GetWorkout(string userId, Guid workoutId)
            => Retrieve(userId, workoutId.ToString());

        public Task<IEnumerable<Workout>> GetWorkoutsForUserAsync(string userId)
        {
            var query = new TableQuery<Workout>();
            query.FilterString = TableQuery.GenerateFilterCondition(
                nameof(Workout.PartitionKey),
                QueryComparisons.Equal,
                userId);
            query.SelectColumns = new[] { nameof(Workout.StartDateTime) }.ToList();
            
            return ExecuteQueryAsync(query);
        }

        public Task DeleteAsync(Guid workoutId, string userId)
        {
            return Table.ExecuteAsync(TableOperation.Delete(
                new Workout(userId, workoutId)
                {
                    ETag = "*"
                }));
        }
    }
}