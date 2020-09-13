using System;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.Payments;

namespace Telegram.Bot.Builder.UpdateHandling
{
    public abstract class BaseUpdateHandler : IUpdateHandler
    {
        public virtual Task HandleAsync(UpdateContext context, Func<UpdateContext, Task> next)
        {
            var upd = context.Update;
            return upd.Type switch
            {
                UpdateType.Message => HandleMessageAsync(context, upd.Message, next),
                UpdateType.InlineQuery => HandleInlineQueryAsync(context, upd.InlineQuery, next),
                UpdateType.ChosenInlineResult => HandleChosenInlineResultAsync(context, upd.ChosenInlineResult, next),
                UpdateType.CallbackQuery => HandleCallbackQueryAsync(context, upd.CallbackQuery, next),
                UpdateType.EditedMessage => HandleEditedMessageAsync(context, upd.EditedMessage, next),
                UpdateType.ChannelPost => HandleChannelPostAsync(context, upd.ChannelPost, next),
                UpdateType.EditedChannelPost => HandleEditedChannelPostAsync(context, upd.EditedChannelPost, next),
                UpdateType.ShippingQuery => HandleShippingQueryAsync(context, upd.ShippingQuery, next),
                UpdateType.PreCheckoutQuery => HandlePreCheckoutQueryAsync(context, upd.PreCheckoutQuery, next),
                UpdateType.Poll => HandlePollAsync(context, upd.Poll, next),
                UpdateType.PollAnswer => HandlePollAnswerAsync(context, upd.PollAnswer, next),
                _ => Task.CompletedTask
            };
        }

        protected virtual Task HandleMessageAsync(UpdateContext context, Message message, Func<UpdateContext, Task> next) =>
            Task.CompletedTask;

        protected virtual Task HandleInlineQueryAsync(UpdateContext context, InlineQuery query, Func<UpdateContext, Task> next) =>
            Task.CompletedTask;

        protected virtual Task HandleChosenInlineResultAsync(UpdateContext context, ChosenInlineResult result, Func<UpdateContext, Task> next) =>
            Task.CompletedTask;

        protected virtual Task HandleCallbackQueryAsync(UpdateContext context, CallbackQuery query, Func<UpdateContext, Task> next) =>
            Task.CompletedTask;

        protected virtual Task HandleEditedMessageAsync(UpdateContext context, Message message, Func<UpdateContext, Task> next) =>
            Task.CompletedTask;

        protected virtual Task HandleChannelPostAsync(UpdateContext context, Message message, Func<UpdateContext, Task> next) =>
            Task.CompletedTask;

        protected virtual Task HandleEditedChannelPostAsync(UpdateContext context, Message message, Func<UpdateContext, Task> next) =>
            Task.CompletedTask;

        protected virtual Task HandleShippingQueryAsync(UpdateContext context, ShippingQuery query, Func<UpdateContext, Task> next) =>
            Task.CompletedTask;

        protected virtual Task HandlePreCheckoutQueryAsync(UpdateContext context, PreCheckoutQuery query, Func<UpdateContext, Task> next) =>
            Task.CompletedTask;

        protected virtual Task HandlePollAsync(UpdateContext context, Poll poll, Func<UpdateContext, Task> next) =>
            Task.CompletedTask;

        protected virtual Task HandlePollAnswerAsync(UpdateContext context, PollAnswer pollAnswer, Func<UpdateContext, Task> next) =>
            Task.CompletedTask;
    }
}
