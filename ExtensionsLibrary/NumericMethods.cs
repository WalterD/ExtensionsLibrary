using System;

namespace ExtensionsLibrary
{
    public static class NumericMethods
    {
        /// <summary>
        /// Determines whether [is positive value].
        /// </summary>
        /// <param name="nullableInteger">The nullable integer.</param>
        /// <returns>
        ///   <c>true</c> if [is positive value] [the specified nullable integer]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsPositiveValue(this int? nullableInteger)
        {
            return nullableInteger.HasValue && nullableInteger.Value > 0;
        }

        /// <summary>
        /// Determines whether [is positive value].
        /// </summary>
        /// <param name="nullableInteger">The nullable integer.</param>
        /// <returns>
        ///   <c>true</c> if [is positive value] [the specified nullable integer]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsPositiveValue(this long? nullableInteger)
        {
            return nullableInteger.HasValue && nullableInteger.Value > 0;
        }

        /// <summary>
        /// Determines whether this instance is prime.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <returns>
        ///   <c>true</c> if the specified number is prime; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsPrime(this int number)
        {
            if (number == 1)
            {
                return false;
            }

            if (number == 2)
            {
                return true;
            }

            for (int i = 2; i < number; ++i)
            {
                if (number % i == 0)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Decimals to money format.
        /// </summary>
        /// <param name="dollarAmount">The dollar amount.</param>
        /// <returns>A string of dollar amount</returns>
        public static string ToCurrencyFormat(this decimal dollarAmount)
        {
            return string.Format("{0:C}", dollarAmount);
        }

        /// <summary>
        /// To the number with commas.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <returns>A number with commas</returns>
        public static string ToNumberWithCommas(decimal number)
        {
            return number.ToString("#,##0.00");
        }

        /// <summary>
        /// ToLong
        /// </summary>
        /// <param name="i">integer</param>
        /// <returns>long</returns>
        public static long ToLong(this int i)
        {
            return (long)i;
        }

        /// <summary>
        /// Provides elapsed times in hours, minutes, seconds and milliseconds
        /// </summary>
        /// <param name="miliseconds">milliseconds</param>
        /// <returns>string</returns>
        public static string ToHoursMinutesSecondsAndMiliseconds(this long miliseconds)
        {
            var timeSpan = TimeSpan.FromMilliseconds(miliseconds);
            return $"{timeSpan.Hours.ToString("D2")}h:{timeSpan.Minutes.ToString("D2")}m:{timeSpan.Seconds.ToString("D2")}s:{timeSpan.Milliseconds.ToString("D2")}ms";
        }
    }
}
