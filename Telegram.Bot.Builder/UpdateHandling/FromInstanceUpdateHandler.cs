using System;
using System.Threading.Tasks;

namespace Telegram.Bot.Builder.UpdateHandling
{
    internal class FromInstanceUpdateHandler : IAdvancedUpdateHandler
    {
        private readonly IUpdateHandler _updateHandler;

        public FromInstanceUpdateHandler(IUpdateHandler updateHandler)
        {
            _updateHandler = updateHandler;
        }

        public Task HandleAsync(UpdateContext context, IServiceProvider _, Func<UpdateContext, Task> next) =>
            _updateHandler.HandleAsync(context, next);
    }
}
