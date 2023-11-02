using System;
using System.Linq;

namespace ExtensionsLibrary
{
    public static class EnumExtensions
    {
        /// <summary>
        /// To the enum.
        /// </summary>
        /// <typeparam name="T">Enum type</typeparam>
        /// <param name="value">The value.</param>
        /// <returns>Enum</returns>
        public static T ToEnum<T>(this string value) // where T : Enum <- enable this in c# 7.3
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        /// <summary>
        /// To the enum.
        /// </summary>
        /// <typeparam name="T">Enum type</typeparam>
        /// <param name="value">The value.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>Enum</returns>
        public static T ToEnum<T>(this string value, T defaultValue) where T : struct
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return defaultValue;
            }

            return Enum.TryParse(value, true, out T result) ? result : defaultValue;
        }

        /// <summary>
        /// Gets the enum attribute.
        /// </summary>
        /// <typeparam name="T">Attribute Type</typeparam>
        /// <param name="value">Attribute value.</param>
        /// <returns>Attribute of type T</returns>
        public static T GetEnumAttribute<T>(this Enum value) where T : Attribute
        {
            object[] attr = value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof(T), false);
            return attr is T[] attribs && attribs.Length > 0 ? attribs[0] : default;
        }

        /// <summary>
        /// Gets the attribute.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="value">The value.</param>
        /// <returns>Attribute of type T</returns>
        public static T GetAttribute<T>(this Enum value) where T : Attribute
        {
            var enumType = value.GetType();
            var name = Enum.GetName(enumType, value);
            return enumType.GetField(name).GetCustomAttributes(false).OfType<T>().SingleOrDefault();
        }
    }
}
