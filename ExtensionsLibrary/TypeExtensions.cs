using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Text.Json;
namespace ExtensionsLibrary
{
    public static class TypeExtensions
    {
        public static bool IsNumericType(this Type type)
        {
            var underlyingType = Nullable.GetUnderlyingType(type);
            TypeCode typeCode = Type.GetTypeCode(underlyingType ?? type);
            switch (typeCode)
            {
                case TypeCode.Int32:
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// DeepClone
        /// </summary>
        public static T DeepClone<T>(this T source)
        {
            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException("The type must be serializable.", nameof(source));
            }

            // Don't serialize a null object, simply return the default for that object
            if (source is null)
            {
                return default;
            }

            using Stream stream = new MemoryStream();
            JsonSerializer.Serialize(stream, source);
            stream.Position = 0;
            var result = JsonSerializer.Deserialize<T>(stream);
            return result;
        }
    }
}
