using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Telegram.Bot.Builder.Extensions
{
    public static class MethodInfoExtensions
    {
        /// <summary>
        /// Invokes method and returns it's result (if any).
        /// </summary>
        /// <param name="method">Target method.</param>
        /// <param name="instance">Instance of 'this' for method or null if method is static.</param>
        /// <param name="args">Arguments for method.</param>
        /// <param name="serviceProvider">If i'th value of <paramref name="args"/> is null then <paramref name="serviceProvider"/> will be used to get i'th argument.</param>
        /// <exception cref="ArgumentNullException"><paramref name="args"/>, <paramref name="serviceProvider"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Amount of arguments doesn't match actual parameters of method.</exception>
        public static object? InvokeWithServices(this MethodInfo method, object? instance, object?[] args, IServiceProvider serviceProvider)
        {
            if (method == null) throw new ArgumentNullException(nameof(method));
            if (args == null) throw new ArgumentNullException(nameof(args));
            if (serviceProvider == null) throw new ArgumentNullException(nameof(serviceProvider));

            var parameterTypes = method.GetParameters();
            var actualArgs = new object[parameterTypes.Length];
            if (actualArgs.Length != args.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(args),
                    $"Method \"{method}\" accepts {actualArgs.Length} arguments, but {nameof(args)}.{nameof(args.Length)} = {args.Length}");
            }
            for(int i = 0; i < actualArgs.Length; ++i)
            {
                actualArgs[i] = args[i] ?? serviceProvider.GetRequiredService(parameterTypes[i].ParameterType);
            }

            return method.Invoke(instance, actualArgs);
        }
    }
}
