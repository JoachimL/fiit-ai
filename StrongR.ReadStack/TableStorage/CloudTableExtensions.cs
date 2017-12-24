using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StrongR.ReadStack.TableStorage
{
    public static class CloudTableExtensions
    {
        /// <summary>
        /// https://stackoverflow.com/questions/24234350/how-to-execute-an-azure-table-storage-query-async-client-version-4-0-1
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="table"></param>
        /// <param name="query"></param>
        /// <param name="ct"></param>
        /// <param name="onProgress"></param>
        /// <returns></returns>
        public static async Task<IEnumerable<T>> ExecuteQueryAsync<T>(
            this CloudTable table, 
            TableQuery<T> query, 
            CancellationToken ct = default(CancellationToken), 
            Action<IList<T>> onProgress = null) where T : ITableEntity, new()
        {
            var runningQuery = new TableQuery<T>()
            {
                FilterString = query.FilterString,
                SelectColumns = query.SelectColumns
            };

            var items = new List<T>();
            TableContinuationToken token = null;

            do
            {
                runningQuery.TakeCount = query.TakeCount - items.Count;

                TableQuerySegment<T> seg = await table.ExecuteQuerySegmentedAsync<T>(runningQuery, token);
                token = seg.ContinuationToken;
                items.AddRange(seg);
                if (onProgress != null) onProgress(items);

            } while (token != null && !ct.IsCancellationRequested && (query.TakeCount == null || items.Count < query.TakeCount.Value));

            return items;
        }
    }
}
