using Telegram.Bot.Types;

namespace Telegram.Bot.Builder.UpdateHandling
{
    public class UpdateContext
    {
        public ITelegramBotClient BotClient { get; }

        public Update Update { get; }

        public BotCommand? Command { get; }

        public UpdateContext
            (
                ITelegramBotClient botClient,
                Update update,
                BotCommand? command
            )
        {
            BotClient = botClient;
            Update = update;
            Command = command;
        }
    }
}
