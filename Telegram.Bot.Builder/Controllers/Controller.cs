using Telegram.Bot.Builder.UpdateHandling;

namespace Telegram.Bot.Builder.Controllers
{
    /// <summary>
    /// Base controller class. Your controllers must inherit it.
    /// </summary>
    public abstract class Controller
    {
        /// <summary>
        /// Current update context.
        /// </summary>
        protected UpdateContext UpdateContext { get; private set; } = null!;

        protected ITelegramBotClient Client => UpdateContext.BotClient;

        internal void Setup(UpdateContext updateContext)
        {
            UpdateContext = updateContext;
        }
    }
}
