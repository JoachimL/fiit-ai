using Microsoft.WindowsAzure.Storage.Table;
using StrongR.ReadStack.Workouts.TableStorage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StrongR.ReadStack.TableStorage
{
    public class TableHandler<T> where T : TableEntity, new()
    {
        private bool _ensured;
        protected readonly CloudTableClient _tableClient;
        private readonly string _tableName;

        public TableHandler(string tableName)
        {
            _tableName = tableName;
        }

        public TableHandler(CloudTableClient cloudTableClient)
        {
            _tableClient = cloudTableClient;
        }

        public CloudTable Table => _tableClient.GetTableReference(TableName);

        public virtual string TableName => _tableName;

        protected async Task EnsureTableExists()
        {
            if (!_ensured)
            {
                await Table.CreateIfNotExistsAsync();
                _ensured = true;
            }
        }

        public async Task InsertOrReplace(T workout)
        {
            await EnsureTableExists();
            TableOperation insert = TableOperation.InsertOrReplace(workout);
            await Table.ExecuteAsync(insert);
        }

        protected virtual async Task<T> Retrieve(string partitionKey, string rowKey)
        {
            TableOperation retrieve = TableOperation.Retrieve<T>(partitionKey, rowKey);
            var retrieved = await Table.ExecuteAsync(retrieve);
            return (T)retrieved.Result;
        }

        public async Task<IEnumerable<T>> ExecuteQueryAsync(TableQuery<T> query)
        {
            await EnsureTableExists();
            return await Table.ExecuteQueryAsync(query);
        }
    }
}
