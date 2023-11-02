using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace ExtensionsLibrary
{
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Returns empty list if the input list is null
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="list">The list.</param>
        /// <returns>List of T</returns>
        public static List<T> ToEmptyIfNull<T>(this List<T> list)
        {
            return list ?? new List<T>();
        }

        /// <summary>
        /// ToEmptyListIfNull
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="enumerable">enumerable</param>
        /// <returns>List of T</returns>
        public static List<T> ToEmptyListIfNull<T>(this IEnumerable<T> enumerable)
        {
            return enumerable == null ? new List<T>() : enumerable.ToList();
        }

        /// <summary>
        /// Determines whether provided list is null or empty
        /// </summary>
        /// <typeparam name="T">List type</typeparam>
        /// <param name="list">list</param>
        /// <returns>true/false</returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> list)
        {
            return list == null || !list.Any();
        }

        /// <summary>
        /// Determines whether this instance has records.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="list">The list.</param>
        /// <returns>
        ///   <c>true</c> if the specified list has records; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasRecords<T>(this IEnumerable<T> list)
        {
            return list != null && list.Any();
        }

        /// <summary>
        /// Converts list to a comma separated list
        /// </summary>
        /// <typeparam name="T">List type</typeparam>
        /// <param name="list">List to be converted</param>
        /// <param name="separator">Optional separator</param>
        /// <returns>Comma separated list</returns>
        public static string ToCommaSeparatedList<T>(this IEnumerable<T> list, string separator = ", ")
        {
            return string.Join(separator, list);
        }

        /// <summary>
        /// Adds item to list if it's not null.
        /// </summary>
        /// <typeparam name="T">List type</typeparam>
        /// <param name="list">The list.</param>
        /// <param name="item">The item.</param>
        /// <returns>IList</returns>
        public static IList<T> AddIfNotNull<T>(this IList<T> list, T item)
        {
            if (item != null)
            {
                list.Add(item);
            }

            return list;
        }

        /// <summary>
        /// Adds the range if not null or empty.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="list">The list.</param>
        /// <param name="listToAdd">The list to add.</param>
        /// <returns>List of type T</returns>
        public static List<T> AddRangeIfNotNullOrEmpty<T>(this List<T> list, IEnumerable<T> listToAdd)
        {
            if (!listToAdd.IsNullOrEmpty())
            {
                list.AddRange(listToAdd);
            }

            return list;
        }

        /// <summary>
        /// Asynchronous ForEach.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="maxDegreeOfParallelism">The dop.</param>
        /// <param name="body">The body.</param>
        /// <returns>Task</returns>
        public static Task ForEachAsync<T>(this IEnumerable<T> source, int maxDegreeOfParallelism, Func<T, Task> body)
        {
            return Task.WhenAll(
                                from partition in Partitioner.Create(source).GetPartitions(maxDegreeOfParallelism)
                                select Task.Run(async delegate
                                {
                                    using (partition)
                                    {
                                        while (partition.MoveNext())
                                        {
                                            await body(partition.Current).ConfigureAwait(false);
                                        }
                                    }
                                }));
        }

        /// <summary>
        /// ParallelForEachAsync https://scatteredcode.net/parallel-foreach-async-in-c/
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="source">source</param>
        /// <param name="body">body</param>
        /// <param name="maxDegreeOfParallelism">maxDegreeOfParallelism</param>
        /// <param name="scheduler">scheduler</param>
        /// <returns>Task</returns>
        public static async Task ParallelForEachAsync<T>(this IAsyncEnumerable<T> source, Func<T, Task> body, int maxDegreeOfParallelism = DataflowBlockOptions.Unbounded, TaskScheduler scheduler = null)
        {
            var options = new ExecutionDataflowBlockOptions
            {
                MaxDegreeOfParallelism = maxDegreeOfParallelism
            };

            if (scheduler != null)
            {
                options.TaskScheduler = scheduler;
            }

            var block = new ActionBlock<T>(body, options);
            await foreach (var item in source)
            {
                block.Post(item);
            }

            block.Complete();
            await block.Completion;
        }

        /// <summary>
        /// Determines whether [contains case insensitive] [the specified search word].
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="searchWord">The search word.</param>
        /// <returns>
        ///   <c>true</c> if [contains case insensitive] [the specified search word]; otherwise, <c>false</c>.
        /// </returns>
        public static bool ContainsCaseInsensitive(this IList<string> list, string searchWord)
        {
            if (list.IsNullOrEmpty() || searchWord == null)
            {
                return false;
            }

            return list.Contains(searchWord, StringComparer.CurrentCultureIgnoreCase);
        }

        /// <summary>
        /// Return a normalized string of parameters suitable for OAuth 1.0 signature base string
        /// as defined by https://tools.ietf.org/html/rfc5849#section-3.4.1.3.2
        /// </summary>
        /// <param name="collection">The list of name/value pairs to normalize.</param>
        /// <returns>A normalized string of parameters.</returns>
        public static string ToNormalizedString(this NameValueCollection collection)
        {
            var keyValuePairList = new List<KeyValuePair<string, string>>();
            var stringList = new List<string>() { "oauth_signature", "realm" };
            foreach (string allKey in collection.AllKeys)
            {
                if (!stringList.Contains(allKey))
                {
                    string str = collection[allKey] ?? string.Empty;
                    keyValuePairList.Add(new KeyValuePair<string, string>(allKey.ToRfc3986EncodedString(), str.ToRfc3986EncodedString()));
                }
            }

            keyValuePairList.Sort((left, right) =>
            {
                if (!left.Key.Equals(right.Key, StringComparison.Ordinal))
                {
                    return string.Compare(left.Key, right.Key, StringComparison.Ordinal);
                }

                return string.Compare(left.Value, right.Value, StringComparison.Ordinal);
            });
            var stringBuilder = new StringBuilder();
            foreach (KeyValuePair<string, string> keyValuePair in keyValuePairList)
            {
                stringBuilder.Append('&').Append(keyValuePair.Key).Append('=').Append(keyValuePair.Value);
            }

            return stringBuilder.ToString().TrimStart('&');
        }

        /// <summary>
        /// Generates HTML table from a list.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="list">list</param>
        /// <param name="footerText">footer</param>
        /// <param name="tableID">tableID</param>
        /// <param name="tableClass">tableClass</param>
        /// <param name="newLine">newLine</param>
        /// <returns>string</returns>
        public static string List2HtmlTable<T>(this List<T> list, string footerText, string tableID, string tableClass, string newLine = "") where T : class
        {
            var sb = new StringBuilder();
            var props = typeof(T).GetProperties();
            var propertyNames = props.Select(p => p.Name).ToList();

            sb.Append($"<table id=\"{tableID}\" class=\"{tableClass}\">{newLine}");
            sb.Append($"<thead>{newLine}");
            sb.Append($"<tr>{newLine}");
            foreach (var propertyName in propertyNames)
            {
                sb.Append($"<th>{propertyName.SplitOnCapitalLetter()}</th>{newLine}");
            }

            sb.Append($"</tr>{newLine}");
            sb.Append($"</thead>{newLine}");
            sb.Append($"<tbody>{newLine}");

            for (int i = 0; i < list.Count; i++)
            {
                sb.Append($"<tr>{newLine}");
                var listItem = list[i];
                for (int k = 0; k < props.Length; k++)
                {
                    var value = props[k].GetValue(listItem);
                    sb.Append($"<td>{value}</td>{newLine}");
                }

                sb.Append($"</tr>{newLine}");
            }

            sb.Append($"</tbody>{newLine}");

            if (!string.IsNullOrWhiteSpace(footerText))
            {
                sb.Append($"<tfoot><tr><td style=\"text-align: center;\" colspan=\"{propertyNames.Count}\">{footerText}</td></tr></tfoot>{newLine}");
            }

            sb.Append($"</table>{newLine}");
            return sb.ToString();
        }

        /// <summary>
        /// ToDataTable
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="data">data</param>
        /// <returns>DataTable</returns>
        public static DataTable ToDataTable<T>(this IEnumerable<T> data)
        {
            var table = new DataTable();
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
            for (int i = 0; i < props.Count; i++)
            {
                table.Columns.Add(props[i].Name, Nullable.GetUnderlyingType(props[i].PropertyType) ?? props[i].PropertyType);
            }

            object[] values = new object[props.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item) ?? DBNull.Value;
                }

                table.Rows.Add(values);
            }

            return table;
        }

        /// <summary>
        /// BulkInsert
        /// </summary>
        /// <typeparam name="T">type</typeparam>
        /// <param name="records">records</param>
        /// <param name="dbConnectionString">dbConnectionString</param>
        /// <param name="tableName">tableName</param>
        /// <param name="schema">schema</param>
        /// <param name="batchSize">batchSize</param>
        /// <returns>Number of inserted records</returns>
        public static async Task<int> BulkInsertAsync<T>(this IEnumerable<T> records, string dbConnectionString, string tableName, string schema = "dbo", int batchSize = 500)
        {
            if (records.IsNullOrEmpty()
                || tableName.IsNullOrWhiteSpace()
                || dbConnectionString.IsNullOrWhiteSpace()
                || schema.IsNullOrWhiteSpace()
                || batchSize <= 0)
            {
                return 0;
            }

            schema = schema.Trim();
            if (!schema.StartsWith('['))
            {
                schema = $"[{schema}]";
            }

            tableName = tableName.Trim();
            if (!tableName.StartsWith('['))
            {
                tableName = $"[{tableName}]";
            }

            int rowsInserted = 0;
            var table = records.ToDataTable();
            if (table != null && table.Rows.Count > 0)
            {
                var bulk = new SqlBulkCopy(dbConnectionString, SqlBulkCopyOptions.TableLock)
                {
                    DestinationTableName = $"{schema}.{tableName}",
                    BatchSize = batchSize
                };
                await bulk.WriteToServerAsync(table);
                bulk.Close();
                rowsInserted = table.Rows.Count;
            }

            table?.Dispose();
            return rowsInserted;
        }

        /// <summary>
        /// Converts IAsyncEnumerable to list
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="asyncEnumerable">asyncEnumerable</param>
        /// <returns>List of T</returns>
        public static async Task<List<T>> ToListAsync<T>(this IAsyncEnumerable<T> asyncEnumerable)
        {
            var list = new List<T>();
            await foreach (var item in asyncEnumerable)
            {
                list.Add(item);
            }

            return list;
        }

        /// <summary>
        /// ToConcurrentBag
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="collection">collection</param>
        /// <returns>Convert collection to ConcurrentBag</returns>
        public static ConcurrentBag<T> ToConcurrentBag<T>(this IEnumerable<T> collection)
        {
            return new ConcurrentBag<T>(collection);
        }

    }
}
