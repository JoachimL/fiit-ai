using Microsoft.WindowsAzure.Storage.Table;
using StrongR.ReadStack.Workouts.TableStorage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StrongR.ReadStack.TableStorage
{
    public class ExerciseRepository
    {
        private CloudTableClient _cloudTableClient;

        public ExerciseRepository(CloudTableClient tableClient)
        {
            _cloudTableClient = tableClient;
        }

        public Task<IEnumerable<Exercise>> GetAllExercisesAsync()
        {
            var table = _cloudTableClient.GetTableReference("exercises");
            return table.ExecuteQueryAsync(
                new TableQuery<Exercise>());
        }
    }
}
