using System;
using Telegram.Bot.Builder.Controllers.Filters;
using Telegram.Bot.Builder.Extensions;

namespace Telegram.Bot.Builder.Controllers.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple =  true, Inherited = true)]
    public class FilterAttribute : Attribute
    {
        public Type Type { get; }

        public FilterAttribute(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            type.IsValidFinalImplementationOfOrThrow<IFilter>();

            Type = type;
        }
    }
}
