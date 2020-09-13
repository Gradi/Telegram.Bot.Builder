using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Telegram.Bot.Builder.Hosting;

namespace Telegram.Bot.Builder.Extensions
{
    public static class HostBuilderExtensions
    {
        public static IHostBuilder ConfigureTelegramBotHost(this IHostBuilder builder, Action<ITelegramBotHostBuilder> configureDelegate)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (configureDelegate == null) throw new ArgumentNullException(nameof(configureDelegate));

            builder.ConfigureServices(sc => sc.AddHostedService<TelegramBotHostedService>());
            var telegramBotHostBuilder = new TelegramBotHostBuilder(builder);
            configureDelegate(telegramBotHostBuilder);
            return builder;
        }
    }
}
