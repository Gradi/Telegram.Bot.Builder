using System;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Builder.UpdateListening;

namespace Telegram.Bot.Builder.Hosting
{
    /// <summary>
    /// Host builder helper for configuring services specific to bot application.
    /// </summary>
    public interface ITelegramBotHostBuilder
    {
        /// <summary>
        /// Configures services for bot application.
        /// </summary>
        ITelegramBotHostBuilder ConfigureServices(Action<IServiceCollection> configureDelegate);

        /// <summary>
        /// Sets bot token to be used.
        /// </summary>
        ITelegramBotHostBuilder UseBotToken(string botToken);

        /// <summary>
        /// Sets <see cref="ITelegramBotClient"/> factory to be used.
        /// </summary>
        ITelegramBotHostBuilder UseTelegramBotClient(Func<ITelegramBotClient> telegramBotClientFactory);

        /// <summary>
        /// Sets <see cref="IStartup"/> class implementation.
        /// </summary>
        ITelegramBotHostBuilder UseStartup<T>() where T : class, IStartup, new();

        /// <summary>
        /// Sets <see cref="IStartup"/> class implementation.
        /// </summary>
        ITelegramBotHostBuilder UseStartup(Func<IStartup> startupFactory);

        /// <summary>
        /// Sets implementation of <see cref="IUpdateListener"/>.
        /// </summary>
        ITelegramBotHostBuilder UseUpdateListener<T>() where T : class, IUpdateListener;
    }
}
