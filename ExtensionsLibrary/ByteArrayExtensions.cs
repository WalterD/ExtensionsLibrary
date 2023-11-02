using System;
using System.IO;

namespace ExtensionsLibrary
{
    public static class ByteArrayExtensions
    {
        public static MemoryStream ToMemoryStream(this byte[] source)
        {
            return new MemoryStream(source);
        }

        public static string ToBase64String(this byte[] source)
        {
            if (source == null || source.Length == 0)
            {
                return string.Empty;
            }

            return Convert.ToBase64String(source);
        }
    }
}
