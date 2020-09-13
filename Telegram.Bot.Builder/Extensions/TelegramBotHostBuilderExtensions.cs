using Telegram.Bot.Builder.Hosting;
using Telegram.Bot.Builder.UpdateListening;

namespace Telegram.Bot.Builder.Extensions
{
    public static class TelegramBotHostBuilderExtensions
    {
        public static ITelegramBotHostBuilder UseDefaultTelegramBotClientUpdateListener(this ITelegramBotHostBuilder builder) =>
            builder.UseUpdateListener<DefaultTelegramBotClientUpdateListener>();
    }
}
