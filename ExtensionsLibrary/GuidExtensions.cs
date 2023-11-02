using System;

namespace ExtensionsLibrary
{
    public static class GuidExtensions
    {
        /// <summary>
        /// GetShortGuid
        /// </summary>
        /// <param name="prefix">prefix</param>
        /// <param name="suffix">suffix</param>
        /// <returns>string</returns>
        public static string GetShortGuid(string prefix = null, string suffix = null)
        {
            var guid = GuidEncoder.Encode(Guid.NewGuid());
            return $"{prefix}{guid}{suffix}";
        }

        /// <summary>
        /// GuidEncoder: https://madskristensen.net/blog/A-shorter-and-URL-friendly-GUID 
        /// </summary>
        public static class GuidEncoder
        {
            public static string Encode(string guidText)
            {
                Guid guid = new Guid(guidText);
                return Encode(guid);
            }

            public static string Encode(Guid guid)
            {
                string enc = Convert.ToBase64String(guid.ToByteArray());
                enc = enc.Replace("/", "_");
                enc = enc.Replace("+", "-");
                return enc.Substring(0, 22);
            }

            public static Guid Decode(string encoded)
            {
                encoded = encoded.Replace("_", "/");
                encoded = encoded.Replace("-", "+");
                byte[] buffer = Convert.FromBase64String(encoded + "==");
                return new Guid(buffer);
            }
        }
    }
}
