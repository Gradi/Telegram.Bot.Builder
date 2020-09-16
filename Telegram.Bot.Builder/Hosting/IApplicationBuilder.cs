using Telegram.Bot.Builder.UpdateHandling;

namespace Telegram.Bot.Builder.Hosting
{
    /// <summary>
    /// An interface for building update handling chain.
    /// </summary>
    public interface IApplicationBuilder
    {
        /// <summary>
        /// Adds update handler that is resolved with DI for every incoming update.
        /// </summary>
        IApplicationBuilder Use<T>() where T : class, IUpdateHandler;

        /// <summary>
        /// Adds a concrete instance of update handler.
        /// </summary>
        IApplicationBuilder Use(IUpdateHandler handler);
    }
}
