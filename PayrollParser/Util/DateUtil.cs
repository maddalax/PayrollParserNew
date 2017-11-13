using System;

namespace PayrollParser.Util
{
    public class DateUtil
    {
        public static string SimpleDate(long millis)
        {
            var ticks = millis;
            var time = TimeSpan.FromMilliseconds(ticks);
            var date = new DateTime(1970, 1, 1) + time;
            return $"{date.Month}/{date.Day}'{date.Year}";
        }

        public static DateTime GetDateTime(long millis)
        {
            var ticks = millis;
            var time = TimeSpan.FromMilliseconds(ticks);
            return new DateTime(1970, 1, 1) + time;
        }
    }
}