using System;

namespace CommentsDemo.Core
{
    public static class DateTimeUtil
    {
        public static string GetInvertedDateTime()
        {
            // Padding zeros to fill the gap. Maximum's length is 19 (9223372036854775807)
            return string.Format("{0:D19}", DateTime.MaxValue.Ticks - DateTime.UtcNow.Ticks);
        }

        public static DateTime GetOriginalDateTime(string invertedDateTimeInString)
        {
           return new DateTime(DateTime.MaxValue.Ticks - long.Parse(invertedDateTimeInString));
        }
    }
}
