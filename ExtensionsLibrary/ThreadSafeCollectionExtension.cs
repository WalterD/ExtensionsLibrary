using System.Collections.Concurrent;
using System.Collections.Generic;

namespace ExtensionsLibrary
{
    public static class ThreadSafeCollectionExtension
    {
        /// <summary>
        /// Adds the range.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="concurrentBag">The concurrent bag.</param>
        /// <param name="collection">The collection.</param>
        /// <returns>ConcurrentBag</returns>
        public static ConcurrentBag<T> AddRange<T>(this ConcurrentBag<T> concurrentBag, IEnumerable<T> collection)
        {
            if (collection != null)
            {
                foreach (var item in collection)
                {
                    concurrentBag.Add(item);
                }
            }

            return concurrentBag;
        }
    }
}
