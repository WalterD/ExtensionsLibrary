using System;

namespace ExtensionsLibrary
{
    public static class DateExtensions
    {
        /// <summary>
        /// Determines whether [is between dates] [the specified start date].
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns>
        ///   <c>true</c> if [is between dates] [the specified start date]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsBetweenDates(this DateTime date, DateTime startDate, DateTime endDate)
        {
            return date.Ticks >= startDate.Ticks && date.Ticks <= endDate.Ticks;
        }

        /// <summary>
        /// ToElapsedTimeInWords
        /// </summary>
        /// <param name="endDateTime">endDateTime</param>
        /// <returns>string</returns>
        public static string ToElapsedTimeInWords(this DateTime? endDateTime)
        {
            if (!endDateTime.HasValue)
            {
                return string.Empty;
            }

            return ToElapsedTimeInWords(endDateTime.Value);
        }

        /// <summary>
        /// ToElapsedTimeInWords
        /// </summary>
        /// <param name="endDateTime">endDateTime</param>
        /// <returns>string</returns>
        public static string ToElapsedTimeInWords(this DateTime endDateTime)
        {
            var ts = new TimeSpan(DateTime.Now.Ticks - endDateTime.Ticks);
            double delta = Math.Abs(ts.TotalSeconds);

            if (delta < 60)
            {
                return "just now";

                // Do not delte below.
                //// return ts.Seconds == 1 ? "one second ago" : ts.Seconds + " seconds ago";
            }

            if (delta < 120)
            {
                return "a minute ago";
            }

            // 60 * 60
            if (delta < 3600)
            {
                return $"{ts.Minutes} minutes ago";
            }

            // 60 * 60 * 2
            if (delta < 7200)
            {
                return "an hour ago";
            }

            // 24 * 60 * 60
            if (delta < 86400)
            {
                return $"{ts.Hours} hours ago";
            }

            // 48 * 60 * 60
            if (delta < 172800)
            {
                return "yesterday";
            }

            // 30 * 24 * 60 * 60
            if (delta < 2592000)
            {
                return $"{ts.Days} days ago";
            }

            // 12 * 30 * 24 * 60 * 60
            if (delta < 31104000)
            {
                int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                return months <= 1 ? "one month ago" : $"{months} months ago";
            }

            int years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
            return years <= 1 ? "one year ago" : $"{years} years ago";
        }

        /// <summary>
        /// ToFormat_MM_dd_yy_hh_mm_ss_tt
        /// </summary>
        public static string ToFormat_MM_dd_yy_hh_mm_ss_tt(this DateTime? datetime, string defaultValue)
        {
            if (datetime.HasValue)
            {
                return datetime.Value.ToFormat_MM_dd_yy_hh_mm_ss_tt();
            }

            return defaultValue;
        }

        /// <summary>
        /// ToFormat_MM_dd_yy_hh_mm_ss_tt
        /// </summary>
        public static string ToFormat_MM_dd_yy_hh_mm_ss_tt(this DateTime datetime)
        {
            return datetime.ToString("MM/dd/yy hh:mm:ss tt");
        }

        /// <summary>
        /// ToFormat_MM_dd_yy_hh_mm
        /// </summary>
        public static string ToFormat_MM_dd_yy_hh_mm(this DateTime datetime)
        {
            return datetime.ToString("MM/dd/yy hh:mm tt");
        }

        /// <summary>
        /// AdjustTime
        /// </summary>
        public static DateTime? AdjustTime(this DateTime? date, int hour, int minute, int second)
        {
            if (date.HasValue)
            {
                date = new DateTime(date.Value.Year, date.Value.Month, date.Value.Day, hour, minute, second);

            }

            return date;
        }
    }
}
