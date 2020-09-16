using System;

namespace Telegram.Bot.Builder.Controllers.Binders
{
    public interface IBinder
    {
        bool IsTypeSupported(Type type);

        BindingResult Bind(string? value, string name, Type type);
    }
}
