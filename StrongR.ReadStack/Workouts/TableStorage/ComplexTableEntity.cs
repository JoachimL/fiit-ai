using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using StrongR.ReadStack.EntityPropertyConverter;
using System.Collections.Generic;

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