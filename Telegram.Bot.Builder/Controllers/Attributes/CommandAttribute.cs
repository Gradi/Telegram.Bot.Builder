using System;

namespace Telegram.Bot.Builder.Controllers.Attributes
{
    /// <summary>
    /// Use this attribute for setting different command name for controller or method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class CommandAttribute : Attribute
    {
        public string Name { get; }

        public CommandAttribute(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name), "Name is null or empty or whitespace.");
            }
            Name = name;
        }
    }
}
