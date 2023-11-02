using System.Text;

namespace ExtensionsLibrary
{
    public static class StringBuilderExtensions
    {
        /// <summary>
        /// Appends the with delimiter.
        /// </summary>
        /// <param name="sb">The StringBuilder.</param>
        /// <param name="value">The value.</param>
        /// <param name="delimiter">The delimiter.</param>
        /// <param name="skipWhiteSpaceValues">if set to <c>true</c> [skip white space values].</param>
        /// <returns>StringBuilder</returns>
        public static StringBuilder AppendWithDelimiter(this StringBuilder sb, string value, string delimiter, bool skipWhiteSpaceValues)
        {
            if (skipWhiteSpaceValues && string.IsNullOrWhiteSpace(value))
            {
                return sb;
            }

            if (sb.Length > 0)
            {
                sb.Append(delimiter);
            }

            sb.Append(value);
            return sb;
        }
    }
}
