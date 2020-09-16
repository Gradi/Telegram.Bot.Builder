using System;
using System.Collections.Generic;

namespace Telegram.Bot.Builder.Extensions
{
    internal static class TypeExtensions
    {
        public static bool IsValidFinalImplementationOf<T>(this Type left) =>
            left.IsValidFinalImplementationOf(typeof(T));

        public static bool IsValidFinalImplementationOf(this Type left, Type right)
        {
            try
            {
                left.IsValidFinalImplementationOfOrThrow(right);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static void IsValidFinalImplementationOfOrThrow<T>(this Type left) =>
            left.IsValidFinalImplementationOfOrThrow(typeof(T));

        public static void IsValidFinalImplementationOfOrThrow(this Type left, Type right)
        {
            var errors = new List<string>();

            if (!right.IsAssignableFrom(left))
                errors.Add("Left type doesn't implement or inherit right type.");

            if (left.IsInterface)
                errors.Add("Left type is an interface.");

            if (left.IsAbstract)
                errors.Add("Left type in an abstract type.");

            if (errors.Count != 0)
            {
                throw new Exception($"Left type \"{left}\" is not valid implementation of right type \"{right}\".{Environment.NewLine}" +
                                    $"{string.Join(Environment.NewLine, errors)}");
            }
        }
    }
}
