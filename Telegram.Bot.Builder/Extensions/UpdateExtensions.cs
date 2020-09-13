using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Builder.Extensions
{
    public static class UpdateExtensions
    {
        public static string? GetMessageTextData(this Update? update) => update?.Type switch
        {
            null => null,
            UpdateType.Message => update.Message.Text,
            UpdateType.InlineQuery => update.InlineQuery.Query,
            UpdateType.ChosenInlineResult => update.ChosenInlineResult.Query,
            UpdateType.CallbackQuery => update.CallbackQuery.Data,
            UpdateType.EditedMessage => update.EditedMessage.Text,
            UpdateType.ChannelPost => update.ChannelPost.Text,
            UpdateType.EditedChannelPost => update.EditedChannelPost.Text,
            _ => null
        };

    }
}
