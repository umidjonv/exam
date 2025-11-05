using System;
using System.Globalization;

namespace EX.Common.Extensions
{
    public static class DateExtension
    {
        /// <summary>
        /// Return the number of milliseconds since 1970/01/01:
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static long GetSystemTime(this DateTime date)
        {
            var diffTime = date.ToLocalTime() - new DateTime(1970, 1, 1, 0, 0, 0);

            return (long)Math.Round(diffTime.TotalSeconds);
        }

        public static long GetTimeNow(this DateTime date)
        {
            var diffTime = date.ToLocalTime() - DateTime.Now.ToLocalTime();

            return (long)Math.Round(diffTime.TotalSeconds);
        }

        public static DateTime? ConvertShortDate(this string s)
        {
            if (string.IsNullOrWhiteSpace(s))
                return null;

            var format = "ddMMyyyy";

            if (s.IndexOf("/", StringComparison.OrdinalIgnoreCase) != -1)
                format = "dd/MM/yyyy";

            if (s.IndexOf(".", StringComparison.OrdinalIgnoreCase) != -1)
                format = "dd.MM.yyyy";

            if (s.IndexOf(",", StringComparison.OrdinalIgnoreCase) != -1)
                format = "dd,MM,yyyy";

            if (s.IndexOf("-", StringComparison.OrdinalIgnoreCase) != -1)
                format = "dd-MM-yyyy";

            return DateTime.TryParseExact(s, format, null, DateTimeStyles.None, out var date) ? date : (DateTime?)null;
        }

        public static DateTime? ConvertLongDate(this string s)
        {
            if (string.IsNullOrWhiteSpace(s))
                return null;

            var format = "ddMMyyyyHHmm";

            if (s.IndexOf("/", StringComparison.OrdinalIgnoreCase) != -1)
                format = "dd/MM/yyyy HH:mm";

            if (s.IndexOf(".", StringComparison.OrdinalIgnoreCase) != -1)
                format = "dd.MM.yyyy HH:mm";

            if (s.IndexOf(",", StringComparison.OrdinalIgnoreCase) != -1)
                format = "dd,MM,yyyy HH:mm";

            if (s.IndexOf("-", StringComparison.OrdinalIgnoreCase) != -1)
                format = "dd-MM-yyyy HH:mm";

            return DateTime.TryParseExact(s, format, null, DateTimeStyles.None, out var date) ? date : (DateTime?)null;
        }

        public static string ConvertFullDate(this DateTime d)
        {
            return d.ToUniversalTime().ToString("dd-MM-yyyy HH:mm:ss");
        }

        public static DateTime ConvertFullDate(this string s)
        {
            var format = "ddMMyyyyHHmmss";

            if (s.IndexOf("/", StringComparison.OrdinalIgnoreCase) != -1)
                format = "dd/MM/yyyy HH:mm:ss";

            if (s.IndexOf(".", StringComparison.OrdinalIgnoreCase) != -1)
                format = "dd.MM.yyyy HH:mm:ss";

            if (s.IndexOf(",", StringComparison.OrdinalIgnoreCase) != -1)
                format = "dd,MM,yyyy HH:mm:ss";

            if (s.IndexOf("-", StringComparison.OrdinalIgnoreCase) != -1)
                format = "dd-MM-yyyy HH:mm:ss";

            return DateTime.ParseExact(s, format, null, DateTimeStyles.None);
        }

    }
}
