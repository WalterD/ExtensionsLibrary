using System;
using System.Collections.Generic;

namespace ExtensionsLibrary
{
    public static class ListExtensions
    {
        public static List<T> AddIfNotExists<T>(this List<T> list, T element)
        {
            if (!list.Contains(element))
            {
                list.Add(element);
            }

            return list;
        }

        public static List<T> AddIfNotExists<T>(this List<T> list, List<T> values)
        {
            if (list != null && values != null)
            {
                foreach (var value in values)
                {
                    if (!list.Contains(value))
                    {
                        list.Add(value);
                    }
                }
            }

            return list;
        }

        public static List<T> RemoveIfExists<T>(this List<T> list, T element)
        {
            if (list.Contains(element))
            {
                list.Remove(element);
            }

            return list;
        }

        public static HashSet<T> AddIfNotExists<T>(this HashSet<T> list, T element)
        {
            if (!list.Contains(element))
            {
                list.Add(element);
            }

            return list;
        }

        public static HashSet<T> RemoveIfExists<T>(this HashSet<T> list, T element)
        {
            if (list.Contains(element))
            {
                list.Remove(element);
            }

            return list;
        }

        /// <summary>
        /// Partition
        /// </summary>
        public static List<List<T>> Partition<T>(this List<T> list, int totalPartitions)
        {
            var partitions = new List<List<T>>();
            int maxSize = (int)Math.Ceiling(list.Count / (double)totalPartitions);
            int k = 0;
            for (int i = 0; i < totalPartitions; i++)
            {
                var partition = new List<T>();
                for (int j = k; j < k + maxSize; j++)
                {
                    if (j >= list.Count)
                    {
                        break;
                    }

                    partition.Add(list[j]);
                }

                k += maxSize;
                partitions.Add(partition);
            }

            return partitions;
        }

        /// <summary>
        /// SearchList
        /// </summary>
        public static List<T> SearchList<T>(this IEnumerable<T> listToSearch, string searchValue, params Func<T, object>[] columns)
        {
            var output = new List<T>();
            if (string.IsNullOrWhiteSpace(searchValue) || listToSearch == null)
            {
                return output;
            }

            searchValue = searchValue.Trim();

            foreach (var listItem in listToSearch)
            {
                foreach (var column in columns)
                {
                    var x = column(listItem);
                    if (x != null && x.ToString().Contains(searchValue, StringComparison.CurrentCultureIgnoreCase))
                    {
                        output.Add(listItem);
                        break;
                    }
                }
            }

            return output;
        }
    }
}
