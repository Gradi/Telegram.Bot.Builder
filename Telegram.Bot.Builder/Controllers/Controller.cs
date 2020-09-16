using Telegram.Bot.Builder.UpdateHandling;

namespace Telegram.Bot.Builder.Controllers
{
    public abstract class Controller
    {
        protected UpdateContext UpdateContext { get; private set; } = null!;

        protected ITelegramBotClient Client => UpdateContext.BotClient;

        internal void Setup(UpdateContext updateContext)
        {
            UpdateContext = updateContext;
        }
    }
}
