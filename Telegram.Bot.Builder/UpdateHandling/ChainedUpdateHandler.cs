using System;
using System.Threading.Tasks;

namespace Telegram.Bot.Builder.UpdateHandling
{
    internal class ChainedUpdateHandler : IAdvancedUpdateHandler
    {
        private readonly IAdvancedUpdateHandler _handler;
        private ChainedUpdateHandler? _next;

        private ChainedUpdateHandler(IAdvancedUpdateHandler handler, ChainedUpdateHandler? next)
        {
            _handler = handler;
            _next = next;
        }

        Task IAdvancedUpdateHandler.HandleAsync(UpdateContext context, IServiceProvider serviceProvider, Func<UpdateContext, Task> _) =>
            HandleAsync(context, serviceProvider);

        public Task HandleAsync(UpdateContext context, IServiceProvider serviceProvider)
        {
            Func<UpdateContext, Task> next = _ => Task.CompletedTask;
            if (_next != null)
            {
                next = ctx => _next.HandleAsync(ctx, serviceProvider);
            }
            return _handler.HandleAsync(context, serviceProvider, next);
        }

        public ChainedUpdateHandler Add(IAdvancedUpdateHandler handler)
        {
            var next = New(handler);
            this._next = next;
            return next;
        }

        public static ChainedUpdateHandler New(IAdvancedUpdateHandler handler) =>
            new ChainedUpdateHandler(handler, null);

    }
}
