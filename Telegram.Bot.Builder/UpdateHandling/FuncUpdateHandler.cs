using System;
using System.Threading.Tasks;

namespace Telegram.Bot.Builder.UpdateHandling
{
    internal class FuncUpdateHandler : IUpdateHandler
    {
        private readonly Func<UpdateContext, Func<UpdateContext, Task>, Task> _handler;

        public FuncUpdateHandler(Func<UpdateContext, Func<UpdateContext, Task>, Task> handler)
        {
            _handler = handler;
        }

        public Task HandleAsync(UpdateContext context, Func<UpdateContext, Task> next) =>
            _handler(context, next);
    }
}
