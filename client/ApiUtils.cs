using System;

namespace client
{
    public static class ApiUtils
    {
        public static string ToISO8601Format(this DateTime dateTime)
            => dateTime.ToString("o"); // yyy-MM-ddTHH\\:mm\\:ss.fffffffzzz
    }
}
