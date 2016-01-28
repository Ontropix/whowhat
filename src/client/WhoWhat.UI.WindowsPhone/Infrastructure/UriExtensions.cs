using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace WhoWhat.UI.WindowsPhone.Infrastructure
{
    public static class UriExtensions
    {
        private static readonly Regex Regex = new Regex(@"[?|&](\w+)=([^?|^&]+)");

        public static IReadOnlyDictionary<string, string> ParseQueryString(this Uri uri)
        {
            var match = Regex.Match(uri.PathAndQuery);
            var paramaters = new Dictionary<string, string>();
            while (match.Success)
            {
                paramaters.Add(match.Groups[1].Value, match.Groups[2].Value);
                match = match.NextMatch();
            }
            return paramaters;
        }
    }
}
