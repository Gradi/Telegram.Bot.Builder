using System;

namespace Telegram.Bot.Builder.Controllers.Binders
{
    /// <summary>
    /// Interface for converting incoming string values to methods arguments.
    /// </summary>
    public interface IBinder
    {
        /// <summary>
        /// Returns true if this binder can bind <paramref name="type"/> type.
        /// </summary>
        bool IsTypeSupported(Type type);

        /// <summary>
        /// Attempts to bind string <paramref name="value"/> to <paramref name="type"/> type.
        /// </summary>
        /// <param name="value">User input.</param>
        /// <param name="name">Argument name. Typically it is argument name in method declaration.</param>
        /// <param name="type">Actual type of method argument.</param>
        BindingResult Bind(string? value, string name, Type type);
    }
}
