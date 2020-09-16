using System;

namespace Telegram.Bot.Builder.Controllers.Binders
{
    public abstract class BaseBinder<T> : IBinder
    {
        public bool IsTypeSupported(Type type) => type == typeof(T);

        public abstract BindingResult Bind(string? value, string name, Type type);
    }
}
