using Microsoft.WindowsAzure.Storage.Table;

namespace StrongR.ReadStack.Workouts.TableStorage
{
    public class ComplexTableEntity : TableEntity
    {
        public ComplexTableEntity(string partitionKey, string rowKey)
            : base(partitionKey, rowKey)
        {

        }

        public ComplexTableEntity()
        {

        }
    }
}