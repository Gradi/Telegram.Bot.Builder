using System;
using System.Threading.Tasks;

namespace Telegram.Bot.Builder.UpdateHandling
{
    /// <summary>
    /// Main interface that handles update and, possibly, passes it to the next handler.
    /// </summary>
    public interface IUpdateHandler
    {
        /// <summary>
        /// Handles incoming update.
        /// </summary>
        /// <param name="context">Update context.</param>
        /// <param name="next">Next handler.</param>
        Task HandleAsync(UpdateContext context, Func<UpdateContext, Task> next);
    }
}
