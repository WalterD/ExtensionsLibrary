using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace ExtensionsLibrary
{
    public static class StringExtensions
    {
        /// <summary>
        /// Determines whether two strings means the same doing case insensitive compare
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="compareTo">The compare to.</param>
        /// <returns>
        ///   <c>true</c> if two strings means the same otherwise, <c>false</c>.
        /// </returns>
        public static bool IsSameAs(this string source, string compareTo)
        {
            source = (source ?? string.Empty).Trim().ToLower();
            compareTo = (compareTo ?? string.Empty).Trim().ToLower();
            return source == compareTo;
        }

        /// <summary>
        /// Remove string from the beginning of another string
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="trimString">trimString</param>
        /// <param name="comparisonType">comparisonType</param>
        /// <returns>string</returns>
        public static string TrimStart(this string source, string trimString, StringComparison comparisonType = StringComparison.OrdinalIgnoreCase)
        {
            if (source == null || trimString == null)
            {
                return source;
            }

            if (source.StartsWith(trimString, comparisonType))
            {
                source = source.Remove(0, trimString.Length);
            }

            return source;
        }

        /// <summary>
        /// Trims the end.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="suffixToRemove">The suffix to remove.</param>
        /// <param name="comparisonType">Type of the comparison.</param>
        /// <returns>string</returns>
        public static string TrimEnd(this string input, string suffixToRemove, StringComparison comparisonType)
        {
            if (input != null && suffixToRemove != null && input.EndsWith(suffixToRemove, comparisonType))
            {
                return input.Substring(0, input.Length - suffixToRemove.Length);
            }
            else
            {
                return input;
            }
        }

        /// <summary>
        /// Left
        /// </summary>
        /// <param name="str">str</param>
        /// <param name="length">length</param>
        /// <returns>string</returns>
        public static string Left(this string str, int length)
        {
            return (!string.IsNullOrEmpty(str) && str.Length >= length) ? str.Substring(0, length) : str;
        }

        /// <summary>
        /// Right
        /// </summary>
        /// <param name="str">str</param>
        /// <param name="length">length</param>
        /// <returns>string</returns>
        public static string Right(this string str, int length)
        {
            return (!string.IsNullOrEmpty(str) && str.Length >= length) ? str.Substring(str.Length - length) : str;
        }

        /// <summary>
        /// Convert string to Int32
        /// </summary>
        /// <param name="s">string</param>
        /// <returns>int</returns>
        public static int ToInt32(this string s)
        {
            return Convert.ToInt32(s);
        }

        /// <summary>
        /// Try parsing the string into an int32.  If the string is not a number and/or cannot be parsed, then return the default value.
        /// </summary>
        /// <param name="s">string to parse</param>
        /// <param name="defaultValue">the int value you want returned in case the string cannot be parsed</param>
        /// <returns>parsed int32</returns>
        public static int ToInt32(this string s, int defaultValue)
        {
            if (!int.TryParse(s, out int i))
            {
                return defaultValue;
            }

            return i;
        }

        /// <summary>
        /// Converts the specified string to titlecase
        /// ref: http://msdn.microsoft.com/en-us/library/system.globalization.textinfo.totitlecase.aspx
        /// For example, if the input string is "war and peace", it will return "War And Peace"
        /// </summary>
        /// <param name="s">input string</param>
        /// <returns>string</returns>
        public static string ConvertToTitleCase(this string s)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                return s;
            }

            var ti = new System.Globalization.CultureInfo("en-US", false).TextInfo;
            return ti.ToTitleCase(s);
        }

        /// <summary>
        /// Converts the specified string to proper case
        /// </summary>
        /// <param name="s">input string</param>
        /// <returns>string</returns>
        public static string ConvertToProperCase(this string s)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                return s;
            }

            string str = s.ToLower().ConvertToTitleCase();

            // detect the ' character, and capitalize the next letter
            int i = str.IndexOf("'");
            if (i >= 0 && i + 1 < str.Length)
            {
                char[] c = str.ToCharArray();
                c[i + 1] = char.ToUpper(c[i + 1]);
                str = new string(c);
            }

            return str;
        }

        /// <summary>
        /// Capitalize first letter
        /// </summary>
        /// <param name="s">input string</param>
        /// <returns>string</returns>
        public static string CapitalizeFirstLetter(this string s)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                return s;
            }

            var newString = s.Substring(0, 1).ToUpper();
            if (s.Length > 1)
            {
                newString += s.Substring(1);
            }

            return newString;
        }

        /// <summary>
        /// Replaces the character at index position.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <param name="index">The index.</param>
        /// <param name="chr">The character.</param>
        /// <returns>string</returns>
        public static string ReplaceCharacterAtIndexPosition(this string s, int index, char chr)
        {
            if (string.IsNullOrEmpty(s))
            {
                return s;
            }

            if (index < 0)
            {
                return s;
            }

            char[] c = s.ToCharArray();
            c = c.ReplaceCharacterAtIndexPosition(index, chr);
            return new string(c);
        }

        /// <summary>
        /// ReplaceCharacterAtIndexPosition
        /// </summary>
        /// <param name="charArray">charArray</param>
        /// <param name="index">index</param>
        /// <param name="chr">chr</param>
        /// <returns>char[]</returns>
        public static char[] ReplaceCharacterAtIndexPosition(this char[] charArray, int index, char chr)
        {
            if (charArray == null || charArray.Length == 0)
            {
                return new char[0];
            }

            if (index < 0 || index >= charArray.Length)
            {
                return charArray;
            }

            charArray[index] = chr;
            return charArray;
        }

        /// <summary>
        /// To the empty if null or whitespace.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns>string</returns>
        public static string ToEmptyIfNullOrWhitespace(this string str)
        {
            return string.IsNullOrWhiteSpace(str) ? string.Empty : str;
        }

        /// <summary>
        /// To the null if empty or whitespace.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns>String or NULL</returns>
        public static string ToNullIfEmptyOrWhitespace(this string str)
        {
            str = str?.Trim();
            str = string.IsNullOrWhiteSpace(str) ? null : str;
            return str;
        }

        /// <summary>
        /// IsPositiveInteger
        /// </summary>
        /// <param name="str">string</param>
        /// <returns>true/false</returns>
        public static bool IsPositiveInteger(this string str)
        {
            return int.TryParse(str, out int i) && i > 0;
        }

        /// <summary>
        /// IsPositiveLong
        /// </summary>
        /// <param name="str">string</param>
        /// <returns>bool</returns>
        public static bool IsPositiveLong(this string str)
        {
            return long.TryParse(str, out long i) && i > 0;
        }

        /// <summary>
        /// Determines whether this instance is numeric.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns>
        ///   <c>true</c> if the specified string is numeric; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNumeric(this string str)
        {
            return str.All(char.IsNumber);
        }

        /// <summary>
        /// Check if string is null or whitespace
        /// </summary>
        /// <param name="s">string to be examined</param>
        /// <returns>true/false</returns>
        public static bool IsNullOrWhiteSpace(this string s)
        {
            return string.IsNullOrWhiteSpace(s);
        }

        /// <summary>
        /// To the long.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns>long</returns>
        public static long ToLong(this string s)
        {
            return Convert.ToInt64(s);
        }

        /// <summary>
        /// Convert string to nullable DateTime
        /// </summary>
        /// <param name="s">String to be converted</param>
        /// <returns>Nullable DateTime</returns>
        public static DateTime? ToDateTime(this string s)
        {
            if (string.IsNullOrWhiteSpace(s) || !DateTime.TryParse(s, out DateTime result))
            {
                return null;
            }
            else
            {
                return result;
            }
        }

        /// <summary>
        /// Convert string to nullable DateTime
        /// </summary>
        /// <param name="s">String to be converted</param>
        /// <returns>Nullable DateTime</returns>
        public static DateTime? ToNullableDateTime(this string s)
        {
            if (string.IsNullOrWhiteSpace(s) || !DateTime.TryParse(s, out DateTime result))
            {
                return null;
            }

            return result;
        }

        /// <summary>
        /// This method splits string, puts spaces in front of each capital Letter, followed by lower case letter
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns>String</returns>
        public static string SplitOnCapitalLetter(this string s)
        {
            return Regex.Replace(s, "((?<=[a-z])[A-Z]|[A-Z](?=[a-z]))", " $1").TrimStart().ConvertToTitleCase();
        }

        public static string[] SplitOnCharacterAndTrim(this string s, char c, StringSplitOptions stringSplitOptions = StringSplitOptions.RemoveEmptyEntries)
        {
            // Remove duplicated spaces.
            var wordList = s.Trim()
                            .Split(new char[] { c, ' ', '\r', '\n', '\t' }, StringSplitOptions.RemoveEmptyEntries)
                            .Where(r => !r.IsNullOrWhiteSpace())
                            .Select(r => r.Trim())
                            .ToArray();
            return wordList;
        }

        /// <summary>
        /// To the memory stream.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns>MemoryStream</returns>
        public static MemoryStream ToMemoryStream(this string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        /// <summary>
        /// To the byte array.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns>Byte array</returns>
        public static byte[] ToByteArray(this string s)
        {
            var memoryStream = new MemoryStream();
            var streamWriter = new StreamWriter(memoryStream);
            streamWriter.Write(s);
            streamWriter.Flush();
            byte[] byteArray = memoryStream.ToArray();
            streamWriter?.Dispose();
            memoryStream?.Dispose();
            return byteArray;
        }

        /// <summary>
        /// Reverses the string.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns>Reversed string</returns>
        public static string ReverseString(this string s)
        {
            char[] arr = s.ToCharArray();
            Array.Reverse(arr);
            return new string(arr);
        }

        /// <summary>
        /// Removes the white space - fastest way.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>string</returns>
        public static string RemoveWhiteSpace(this string input)
        {
            if (input == null)
            {
                return null;
            }

            char[] src = input.ToCharArray();
            int index = 0;
            for (int i = 0; i < input.Length; i++)
            {
                var ch = src[i];
                switch (ch)
                {
                    case '\u0020':
                    case '\u00A0':
                    case '\u1680':
                    case '\u2000':
                    case '\u2001':
                    case '\u2002':
                    case '\u2003':
                    case '\u2004':
                    case '\u2005':
                    case '\u2006':
                    case '\u2007':
                    case '\u2008':
                    case '\u2009':
                    case '\u200A':
                    case '\u202F':
                    case '\u205F':
                    case '\u3000':
                    case '\u2028':
                    case '\u2029':
                    case '\u0009':
                    case '\u000A':
                    case '\u000B':
                    case '\u000C':
                    case '\u000D':
                    case '\u0085':
                        continue;
                    default:
                        src[index++] = ch;
                        break;
                }
            }

            return new string(src, 0, index);
        }

        /// <summary>
        /// Replaces the character at.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="index">The index.</param>
        /// <param name="newChar">The new character.</param>
        /// <returns>String</returns>
        public static string ReplaceCharacterAt(this string input, int index, char newChar)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }

            char[] chars = input.ToCharArray();
            chars[index] = newChar;
            return new string(chars);
        }

        /// <summary>
        /// Replaces all characters.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <param name="characterToReplaceWith">The character to replace with.</param>
        /// <returns>String</returns>
        public static string ReplaceAllCharacters(this string s, char characterToReplaceWith)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            else
            {
                return string.Empty.PadRight(s.Length, characterToReplaceWith);
            }
        }

        /// <summary>
        /// Replaces the ignore case.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <param name="pattern">The pattern.</param>
        /// <param name="replacement">The replacement.</param>
        /// <returns>String</returns>
        public static string ReplaceIgnoreCase(this string s, string pattern, string replacement)
        {
            if (string.IsNullOrEmpty(s) || string.IsNullOrEmpty(pattern) || replacement == null)
            {
                return s;
            }

            int count, position0, position1;
            count = position0 = 0;
            string upperString = s.ToUpper();
            string upperPattern = pattern.ToUpper();
            int inc = (s.Length / pattern.Length) * (replacement.Length - pattern.Length);
            char[] chars = new char[s.Length + Math.Max(0, inc)];
            while ((position1 = upperString.IndexOf(upperPattern, position0)) != -1)
            {
                for (int i = position0; i < position1; ++i)
                {
                    chars[count++] = s[i];
                }

                for (int i = 0; i < replacement.Length; ++i)
                {
                    chars[count++] = replacement[i];
                }

                position0 = position1 + pattern.Length;
            }

            if (position0 == 0)
            {
                return s;
            }

            for (int i = position0; i < s.Length; ++i)
            {
                chars[count++] = s[i];
            }

            return new string(chars, 0, count);
        }

        /// <summary>
        /// Determines whether this instance has value.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns>
        ///   <c>true</c> if the specified s has value; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasValue(this string s)
        {
            return !string.IsNullOrWhiteSpace(s);
        }

        /// <summary>
        /// Encodes string using UTF8 object
        /// </summary>
        /// <param name="s">String to be encoded</param>
        /// <returns>Encoded string</returns>
        public static string UrlEncode(this string s)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                return string.Empty;
            }

            return HttpUtility.UrlEncode(s.Trim(), System.Text.Encoding.UTF8);
        }

        /// <summary>
        /// Fixes the filename extension.
        /// </summary>
        /// <param name="extension">The extension.</param>
        /// <returns>Filename extension</returns>
        public static string FixFilenameExtension(this string extension)
        {
            if (extension.IsNullOrWhiteSpace())
            {
                return extension;
            }

            if (extension.StartsWith('.'))
            {
                return extension;
            }

            return $".{extension}";
        }

        /// <summary>
        /// To the RFC3986 encoded string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>Rfc3986 Encoded String</returns>
        public static string ToRfc3986EncodedString(this string value)
        {
            string[] uriRfc3986CharsToEscape = new string[5] { "!", "*", "'", "(", ")" };
            var stringBuilder = new StringBuilder(Uri.EscapeDataString(value));
            foreach (string oldValue in uriRfc3986CharsToEscape)
            {
                stringBuilder.Replace(oldValue, Uri.HexEscape(oldValue[0]));
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Base64s the encode.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns>Encoded string</returns>
        public static string Base64Encode(this string str)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(str));
        }

        /// <summary>
        /// Base64s the decode.
        /// </summary>
        /// <param name="base64EncodedData">The base64 encoded data.</param>
        /// <returns>Decoded string</returns>
        public static string Base64Decode(this string base64EncodedData)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(base64EncodedData));
        }

        /// <summary>
        /// IsValidEmail
        /// </summary>
        /// <param name="email">email</param>
        /// <returns>bool</returns>
        public static bool IsValidEmail(this string email)
        {
            return !string.IsNullOrWhiteSpace(email) && new EmailAddressAttribute().IsValid(email);
        }

        /// <summary>
        /// Uppercases the first letter.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns>string</returns>
        public static string UppercaseFirstLetter(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }

            char[] a = str.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }

        /// <summary>
        /// ToIso8601Date
        /// </summary>
        /// <param name="dateTime">dateTime</param>
        /// <returns>ISO 8601 Date</returns>
        public static string ToIso8601DateString(this DateTime? dateTime)
        {
            return dateTime.HasValue ? ToIso8601DateString(dateTime.Value) : null;
        }

        /// <summary>
        /// ToIso8601Date
        /// </summary>
        /// <param name="dateTime">dateTime</param>
        /// <returns>ISO 8601 Date</returns>
        public static string ToIso8601DateString(this DateTime dateTime)
        {
            return dateTime.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");
        }

        /// <summary>
        /// Reduce
        /// </summary>
        /// <param name="s">input string</param>
        /// <param name="maxFinalLength">count</param>
        /// <param name="endings">endings</param>
        /// <returns>Reduced string</returns>
        public static string Reduce(this string s, int maxFinalLength, string endings = "...")
        {
            if (maxFinalLength > s.Length || string.IsNullOrEmpty(endings))
            {
                return s;
            }

            maxFinalLength -= endings.Length;
            return $"{s.Substring(0, maxFinalLength)}{endings}";
        }

        /// <summary>
        /// Strips HTML tags from a string.
        /// </summary>
        /// <param name="s">input string</param>
        /// <returns>string</returns>
        public static string StripHTML(this string s)
        {
            if (s.IsNullOrWhiteSpace())
            {
                return s;
            }

            // Replace HTML tags with spaces.
            var singleSpce = " ";
            Regex rgx = new Regex(@"<(.|\n)*?>");
            string result = rgx.Replace(s, singleSpce);

            // Remove duplicated spaces.
            var wordList = result.Trim()
                                 .Split(new char[] { ' ', '\r', '\n', '\t' }, StringSplitOptions.RemoveEmptyEntries)
                                 .Where(r => !r.IsNullOrWhiteSpace())
                                 .Select(r => r.Trim())
                                 .ToList();

            // Put back the whole text.
            return string.Join(" ", wordList);
        }

        /// <summary>
        /// RemoveControlCharacters
        /// </summary>
        /// <param name="s">s</param>
        /// <returns>string</returns>
        public static string RemoveControlCharacters(this string s)
        {
            if (s == null)
            {
                return null;
            }

            return new string(s.Where(c => !char.IsControl(c)).ToArray());
        }

        /// <summary>
        /// RemoveNonAsciiCharacters
        /// </summary>
        /// <param name="s">s</param>
        /// <returns>string</returns>
        public static string RemoveNonAsciiCharacters(this string s)
        {
            return s == null ? null : (new Regex("[^ -~]+")).Replace(s, string.Empty);
        }

        /// <summary>
        /// CountWords
        /// </summary>
        /// <param name="s">s</param>
        /// <returns>string</returns>
        public static int CountWords(this string s)
        {
            if (s.IsNullOrWhiteSpace())
            {
                return 0;
            }

            char[] delimiters = new char[] { ' ', '\r', '\n', '\t' };
            return s.Split(delimiters, StringSplitOptions.RemoveEmptyEntries).Count(w => w.Length > 1);
        }

        /// <summary>
        /// Convert string array of numbers to list of integers
        /// </summary>
        /// <param name="s">s</param>
        /// <returns>List of integers</returns>
        public static List<int> ToListOfIntegers(this string[] s)
        {
            return s.Select(int.Parse).ToList();
        }

        /// <summary>
        /// Check if filename is valid.
        /// </summary>
        /// <param name="filename">filename</param>
        /// <returns>bool</returns>
        public static bool IsFilenameValid(this string filename)
        {
            filename = (filename ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(filename))
            {
                return false;
            }

            var hasInvalidCharacters = filename.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0;
            return !hasInvalidCharacters;
        }

        /// <summary>
        /// ToInt32List
        /// </summary>
        /// <param name="list">list</param>
        /// <returns>List of integers</returns>
        public static List<int> ToInt32List(this IEnumerable<string> list)
        {
            return list.Select(int.Parse).ToList();
        }

        /// <summary>
        /// ConvertNewLineToHtmlBreakTag
        /// </summary>
        /// <param name="text">text</param>
        /// <returns>string</returns>
        public static string ConvertNewLineToHtmlBreakTag(this string text)
        {
            return text?.Replace(Environment.NewLine, "<br />");
        }

        /// <summary>
        /// ReplaceNullOrWhitespace
        /// </summary>
        public static string ReplaceIfNullOrWhitespace(this string str, string nullReplacement)
        {
            return string.IsNullOrWhiteSpace(str) ? nullReplacement : str;
        }
    }
}