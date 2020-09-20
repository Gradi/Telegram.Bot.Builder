using Telegram.Bot.Types;

namespace Telegram.Bot.Builder.Webhook.Internals
{
    internal interface IUpdateAccepter
    {
        void Accept(Update update);
    }
}
