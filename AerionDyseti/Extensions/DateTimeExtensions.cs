using System;

namespace AerionDyseti.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ToUnixTimestamp(this DateTime d)
        {
            var epoch = d - new DateTime(1970, 1, 1, 0, 0, 0);
            return ((int) epoch.TotalSeconds).ToString();
        }
    }
}