using System;

namespace Gov.Jag.Embc.Public.Utils
{
    public class TimeZoneConverter
    {
        public static string GetLocalDate(DateTime dateTime)
        {
            var pstdateTime = GetTimeZoneDateTime(dateTime);

            return pstdateTime.ToString("MMM-dd-yyyy");
        }

        public static DateTime GetLocalDateTime(DateTime dateTime)
        {
            return GetTimeZoneDateTime(dateTime);
        }

        public static string GetLocalTime(DateTime dateTime)
        {
            var pstdateTime = GetTimeZoneDateTime(dateTime);

            return pstdateTime.ToString("h:mm tt"); // eg, 5:30 PM
        }

        public static string GetLocalTime24h(DateTime dateTime)
        {
            var pstdateTime = GetTimeZoneDateTime(dateTime);

            return pstdateTime.ToString("HH:mm"); // eg, 17:30
        }

        public static string GetLocalDay(string format = "MMM-dd-yyyy")
        {
            var localNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, GetPSTTimeZoneInfo());
            return localNow.ToString(format);
        }

        public static string GetFormatedLocalDateTime(DateTime dateTime)
        {
            //Tue, 11 Jun 2019 11:36:22 PDT
            var format = "ddd dd MMM yyyy H:mm:ss";
            var pstDateTime = GetTimeZoneDateTime(dateTime);
            if (GetPSTTimeZoneInfo().IsDaylightSavingTime(pstDateTime))
            {
                return pstDateTime.ToString($"{format} PDT");
            }
            return pstDateTime.ToString($"{format} PST");
        }

        private static DateTime GetTimeZoneDateTime(DateTime dateTime)
        {
            var pstTime = TimeZoneInfo.ConvertTimeFromUtc(dateTime, GetPSTTimeZoneInfo());

            return pstTime;
        }

        private static TimeZoneInfo GetPSTTimeZoneInfo()
        {
            var timeZoneName = "Pacific Standard Time";
            if (PlatformHelper.IsLinux)
            {
                timeZoneName = "Canada/Pacific";
            }

            return TimeZoneInfo.FindSystemTimeZoneById(timeZoneName);
        }
    }
}
