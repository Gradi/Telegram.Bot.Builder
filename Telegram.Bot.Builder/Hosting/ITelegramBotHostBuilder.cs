using System;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Builder.UpdateListening;

namespace Telegram.Bot.Builder.Hosting
{
    public interface ITelegramBotHostBuilder
    {
        ITelegramBotHostBuilder ConfigureServices(Action<IServiceCollection> configureDelegate);

        ITelegramBotHostBuilder UseBotToken(string botToken);

        ITelegramBotHostBuilder UseTelegramBotClient(Func<ITelegramBotClient> telegramBotClientFactory);

        ITelegramBotHostBuilder UseStartup<T>() where T : class, IStartup, new();

        ITelegramBotHostBuilder UseStartup(Func<IStartup> startupFactory);

        ITelegramBotHostBuilder UseUpdateListener<T>() where T : class, IUpdateListener;
    }
}
