using System;
using System.Linq;

namespace ExtensionsLibrary
{
    public static class AttributeExtensions
    {
        /// <summary>
        /// GetAttributeFrom
        /// </summary>
        /// <typeparam name="T">T Attribute</typeparam>
        /// <param name="instance">instance</param>
        /// <param name="propertyName">propertyName</param>
        /// <returns>Attribute</returns>
        public static T GetAttributeFrom<T>(this object instance, string propertyName) where T : Attribute
        {
            var attrType = typeof(T);
            var property = instance.GetType().GetProperty(propertyName);
            return (T)property.GetCustomAttributes(attrType, false).First();
        }

        public static TValue GetAttributeValue<TAttribute, TValue>(this Type type, Func<TAttribute, TValue> valueSelector) where TAttribute : Attribute
        {
            var attr = type.GetAttribute<TAttribute>();
            return attr != null ? valueSelector(attr) : default;
        }

        public static TAttribute GetAttribute<TAttribute>(this Type type) where TAttribute : Attribute
        {
            return type.GetCustomAttributes(typeof(TAttribute), false).FirstOrDefault() as TAttribute;
        }
    }
}
