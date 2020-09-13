using Telegram.Bot.Builder.UpdateHandling;

namespace Telegram.Bot.Builder.Hosting
{
    public interface IApplicationBuilder
    {
        IApplicationBuilder Use<T>() where T : class, IUpdateHandler;

        IApplicationBuilder Use(IUpdateHandler handler);
    }
}
