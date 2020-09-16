using System;

namespace Telegram.Bot.Builder.Controllers.Attributes
{
    /// <summary>
    /// Marks controller's method as default command.
    /// Default command is a command without subcommand.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited =  false)]
    public class DefaultCommandAttribute : Attribute {}
}
