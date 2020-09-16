using System;

namespace Telegram.Bot.Builder.Controllers.Attributes
{
    /// <summary>
    /// Add this attribute to controller or method to mark it as non command, thus, making it not available to end user.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class IgnoreAttribute : Attribute {}
}
