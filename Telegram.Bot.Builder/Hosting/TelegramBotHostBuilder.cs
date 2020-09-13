using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Telegram.Bot.Builder.UpdateListening;

namespace Telegram.Bot.Builder.Hosting
{
    public class TelegramBotHostBuilder : ITelegramBotHostBuilder
    {
        private readonly IHostBuilder _hostBuilder;

        public TelegramBotHostBuilder(IHostBuilder hostBuilder)
        {
            _hostBuilder = hostBuilder ?? throw new ArgumentNullException(nameof(hostBuilder));
        }

        public ITelegramBotHostBuilder ConfigureServices(Action<IServiceCollection> configureDelegate)
        {
            if (configureDelegate == null) throw new ArgumentNullException(nameof(configureDelegate));

            _hostBuilder.ConfigureServices(configureDelegate);
            return this;
        }

        public ITelegramBotHostBuilder UseBotToken(string botToken)
        {
            if (string.IsNullOrWhiteSpace(botToken))
                throw new ArgumentNullException(nameof(botToken), "Bot token is null or empty or whitespace.");

            return UseTelegramBotClient(() => new TelegramBotClient(botToken));
        }

        public ITelegramBotHostBuilder UseTelegramBotClient(Func<ITelegramBotClient> telegramBotClientFactory)
        {
            if (telegramBotClientFactory == null) throw new ArgumentNullException(nameof(telegramBotClientFactory));

            var key = Key(nameof(UseTelegramBotClient));
            if (_hostBuilder.Properties.ContainsKey(key))
            {
                throw new InvalidOperationException($"You can only use single \"{nameof(ITelegramBotClient)}\"");
            }

            _hostBuilder.Properties.Add(key, null);
            ConfigureServices(sc => sc.AddSingleton<ITelegramBotClient>(_ => telegramBotClientFactory()));
            return this;
        }

        public ITelegramBotHostBuilder UseStartup<T>() where T : class, IStartup, new() =>
            UseStartup(() => new T());

        public ITelegramBotHostBuilder UseStartup(Func<IStartup> startupFactory)
        {
            if (startupFactory == null) throw new ArgumentNullException(nameof(startupFactory));

            var key = Key(nameof(UseStartup));
            if (_hostBuilder.Properties.ContainsKey(key))
            {
                throw new InvalidOperationException("You can only use single startup class.");
            }

            _hostBuilder.Properties.Add(key, null);
            var startupDescriptor = new StartupDescriptor(startupFactory);
            ConfigureServices(sc =>
                {
                    sc.AddSingleton<IStartupDescriptor>(startupDescriptor);
                })
            .ConfigureServices(startupDescriptor.ConfigureServices);
            return this;

        }

        public ITelegramBotHostBuilder UseUpdateListener<T>() where T : class, IUpdateListener
        {
            var key = Key(nameof(UseUpdateListener));
            if (_hostBuilder.Properties.ContainsKey(key))
            {
                throw new InvalidOperationException("You can only use single update listener.");
            }

            _hostBuilder.Properties.Add(key, null);
            ConfigureServices(sc => sc.AddSingleton<IUpdateListener, T>());
            return this;
        }

        private object Key(object key) => Tuple.Create(this, key);
    }
}
