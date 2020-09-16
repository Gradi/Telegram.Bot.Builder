using System;

namespace Telegram.Bot.Builder.Extensions
{
    internal static class StringExtensions
    {
        public static string? TrimEnd(this string? str, string? stringToTrim, StringComparison stringComparison = StringComparison.InvariantCulture)
        {
            if (str == null || stringToTrim == null || !str.EndsWith(stringToTrim))
                return str;

            return str.Substring(0, str.LastIndexOf(stringToTrim, stringComparison));
        }
    }
}
