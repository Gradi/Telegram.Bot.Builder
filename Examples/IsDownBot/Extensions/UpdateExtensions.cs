using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace IsDownBot.Extensions
{
    public static class UpdateExtensions
    {
        public static int? GetUserId(this Update? update) => update?.Type switch
        {
            UpdateType.Message => update.Message.From.Id,
            UpdateType.EditedMessage => update.EditedMessage.From.Id,
            UpdateType.InlineQuery => update.InlineQuery.From.Id,
            UpdateType.ChosenInlineResult => update.ChosenInlineResult.From.Id,
            UpdateType.CallbackQuery => update.CallbackQuery.From.Id,
            UpdateType.ChannelPost => update.ChannelPost.From.Id,
            UpdateType.EditedChannelPost => update.EditedChannelPost.From.Id,
            _ => null
        };
    }
}
