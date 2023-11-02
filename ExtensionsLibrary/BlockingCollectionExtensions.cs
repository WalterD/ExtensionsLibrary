using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace ExtensionsLibrary
{
    /// <summary>
    /// https://blogs.msdn.microsoft.com/pfxteam/2010/04/06/parallelextensionsextras-tour-4-blockingcollectionextensions/
    /// </summary>
    public static class BlockingCollectionExtensions
    {


        public static void AddFromEnumerable<T>(this BlockingCollection<T> target, IEnumerable<T> source, bool completeAddingWhenDone)
        {
            try
            {
                foreach (var item in source)
                {
                    if (item != null)
                    {
                        target.Add(item);
                    }
                }
            }
            finally
            {
                if (completeAddingWhenDone)
                {
                    target.CompleteAdding();
                }
            }
        }


        public static Partitioner<T> GetConsumingPartitioner<T>(this BlockingCollection<T> collection)
        {
            return new BlockingCollectionPartitioner<T>(collection);
        }


        private class BlockingCollectionPartitioner<T> : Partitioner<T>
        {
            public override bool SupportsDynamicPartitions { get { return true; } }
            private BlockingCollection<T> _collection;

            internal BlockingCollectionPartitioner(BlockingCollection<T> collection)
            {
                _collection = collection ?? throw new ArgumentNullException("collection");
            }


            public override IList<IEnumerator<T>> GetPartitions(int partitionCount)
            {
                if (partitionCount < 1)
                {
                    throw new ArgumentOutOfRangeException("partitionCount");
                }

                var dynamicPartitioner = GetDynamicPartitions();
                return Enumerable.Range(0, partitionCount).Select(_ => dynamicPartitioner.GetEnumerator()).ToArray();
            }


            public override IEnumerable<T> GetDynamicPartitions()
            {
                return _collection.GetConsumingEnumerable();
            }
        }


    }
}
