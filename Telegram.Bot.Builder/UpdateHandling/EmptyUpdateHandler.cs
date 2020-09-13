using System;
using System.Threading.Tasks;

namespace Telegram.Bot.Builder.UpdateHandling
{
    internal class EmptyUpdateHandler : IAdvancedUpdateHandler, IUpdateHandler
    {
        Task IUpdateHandler.HandleAsync(UpdateContext _, Func<UpdateContext, Task> __) => Task.CompletedTask;

        Task IAdvancedUpdateHandler.HandleAsync(UpdateContext _, IServiceProvider __, Func<UpdateContext, Task> ___) =>
            Task.CompletedTask;
    }
}
