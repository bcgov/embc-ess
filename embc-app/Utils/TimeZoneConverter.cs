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

        public static string GetLocalTime(DateTime dateTime)
        {
            var pstdateTime = GetTimeZoneDateTime(dateTime);

            return pstdateTime.ToString("h:mm tt");
        }

        private static DateTime GetTimeZoneDateTime(DateTime dateTime)
        {
            var timeZoneName = "Pacific Standard Time";
            if (PlatformHelper.IsLinux)
            {
                timeZoneName = "Canada/Pacific";
            }

            var pst = TimeZoneInfo.FindSystemTimeZoneById(timeZoneName);
            var pstTime = TimeZoneInfo.ConvertTimeFromUtc(dateTime, pst);

            return pstTime;
        }
    }
}
