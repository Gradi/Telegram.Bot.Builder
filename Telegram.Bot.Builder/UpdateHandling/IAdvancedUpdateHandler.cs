using System;
using System.Threading.Tasks;

namespace Telegram.Bot.Builder.UpdateHandling
{
    internal interface IAdvancedUpdateHandler
    {
        Task HandleAsync(UpdateContext context, IServiceProvider serviceProvider, Func<UpdateContext, Task> next);
    }
}
