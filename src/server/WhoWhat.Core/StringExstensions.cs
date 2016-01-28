using System;

namespace WhoWhat.Core
{
    public static class StringExstensions
    {
        public static string Left(this string str, int count)
        {
            if (string.IsNullOrEmpty(str) || count < 1)
                return string.Empty;
            else
                return str.Substring(0, Math.Min(count, str.Length));
        }
    }
}
