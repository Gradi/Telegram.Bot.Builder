using System;
using Telegram.Bot.Builder.Controllers.Binders;
using Telegram.Bot.Builder.Extensions;

namespace Telegram.Bot.Builder.Controllers.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
    public class BinderAttribute : Attribute
    {
        public Type Type { get; }

        public BinderAttribute(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            type.IsValidFinalImplementationOfOrThrow<IBinder>();

            Type = type;
        }
    }
}
