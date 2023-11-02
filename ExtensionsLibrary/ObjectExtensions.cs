using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace ExtensionsLibrary
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// Trims all string properties of an object, not including children.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="input">The input.</param>
        /// <returns>Object</returns>
        public static T TrimStringProperties<T>(this T input)
        {
            if (input == null)
            {
                return input;
            }

            IEnumerable<PropertyInfo> stringProperties = input.GetType()
                                                                .GetProperties()
                                                                .Where(p => p.PropertyType == typeof(string));

            foreach (PropertyInfo stringProperty in stringProperties)
            {
                string currentValue = (string)stringProperty.GetValue(input, null);
                if (currentValue != null && stringProperty.CanWrite)
                {
                    stringProperty.SetValue(input, currentValue.Trim(), null);
                }
            }

            return input;
        }

        /// <summary>
        /// Trim properties of each element in a collection
        /// </summary>
        /// <typeparam name="T">Collection type</typeparam>
        /// <param name="inputCollection">inputCollection</param>
        /// <returns>Collection</returns>
        public static IEnumerable<T> TrimStringPropertiesOfElements<T>(this IEnumerable<T> inputCollection)
        {
            if (inputCollection == null || !inputCollection.Any())
            {
                return inputCollection;
            }

            foreach (var item in inputCollection)
            {
                item.TrimStringProperties();
            }

            return inputCollection;
        }

        /// <summary>
        /// Convert object to query string. This only works 1 level deep.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>Query string</returns>
        public static string ToQueryString(this object obj)
        {
            var properties = from p in obj.GetType().GetProperties()
                             select p.Name + "=" + HttpUtility.UrlEncode(p.GetValue(obj, null)?.ToString());
            string queryString = string.Join("&", properties.ToArray());
            return queryString;
        }

        /// <summary>
        /// CompareTo
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="objBeforeUpdate">obj1</param>
        /// <param name="objAfterUpdate">obj2</param>
        /// <returns>ObjectComare</returns>
        public static ObjectComare CompareTo<T>(this T objBeforeUpdate, T objAfterUpdate)
        {
            var objectCompare = new ObjectComare(objBeforeUpdate, objAfterUpdate);
            return objectCompare;
        }

        /// <summary>
        /// ObjectComare
        /// </summary>
        public class ObjectComare
        {
            public ObjectComare(object objectBeforeUpdate, object objectAfterUpdate)
            {
                Type t = objectBeforeUpdate.GetType();
                ObjectTypeName = t.Name;
                ObjectBeforeUpdate = objectBeforeUpdate;
                PropertyUpdates = new List<PropertyUpdate>();
                foreach (var property in t.GetProperties())
                {
                    var valueBeforeUpdate = property.GetValue(objectBeforeUpdate);
                    var valueAfterUpdate = property.GetValue(objectAfterUpdate);
                    if (valueBeforeUpdate != valueAfterUpdate && (valueBeforeUpdate == null || !valueBeforeUpdate.Equals(valueAfterUpdate)))
                    {
                        PropertyUpdates.Add(new PropertyUpdate()
                        {
                            PropertyName = property.Name,
                            ValueBeforeUpdate = valueBeforeUpdate,
                            ValueAfterUpdate = valueAfterUpdate
                        });
                    }
                }
            }

            public object ObjectBeforeUpdate { get; set; }

            public string ObjectTypeName { get; set; }

            public List<PropertyUpdate> PropertyUpdates { get; set; }

            /// <summary>
            /// PropertyDifference class
            /// </summary>
            public class PropertyUpdate
            {
                public string PropertyName { get; set; }

                public object ValueBeforeUpdate { get; set; }

                public object ValueAfterUpdate { get; set; }
            }
        }
    }
}
