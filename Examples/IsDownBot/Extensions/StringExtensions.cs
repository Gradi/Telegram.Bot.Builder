using System;

namespace IsDownBot.Extensions
{
    public static class StringExtensions
    {
        public static bool IsValidUri(this string str) => Uri.TryCreate(str, UriKind.Absolute, out _);
    }
}
