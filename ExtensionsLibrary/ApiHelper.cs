using System;
using System.Text;
using System.Web;

namespace ExtensionsLibrary
{
    public static class APIHelper
    {
        public static string ToUTCString(this DateTime dateTime)
        {
            return dateTime.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
        }

        public static string AppendToPath(this string path1, string path2, char separator)
        {
            char[] chars = { ' ', separator };
            path1 = (path1 ?? string.Empty).Trim().TrimEnd(chars);
            path2 = (path2 ?? string.Empty).Trim().TrimStart(chars);
            return $"{path1}{separator}{path2}";
        }

        public static void AppendUrlEncoded(this StringBuilder sb, string name, string value, Encoding encoding = null)
        {
            if (sb.Length != 0)
            {
                sb.Append("&");
            }

            if (encoding != null)
            {
                sb.Append($"{HttpUtility.UrlEncode(name, encoding)}={HttpUtility.UrlEncode(value, encoding)}");
            }
            else
            {
                sb.Append($"{HttpUtility.UrlEncode(name)}={HttpUtility.UrlEncode(value)}");
            }
        }
    }
}
