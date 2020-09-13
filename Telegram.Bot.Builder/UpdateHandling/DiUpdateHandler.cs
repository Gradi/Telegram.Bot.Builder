using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Telegram.Bot.Builder.UpdateHandling
{
    internal class DiUpdateHandler :  IAdvancedUpdateHandler
    {
        private readonly Type _updateHandlerType;

        public DiUpdateHandler(Type updateHandlerType)
        {
            _updateHandlerType = updateHandlerType;
        }

        public Task HandleAsync(UpdateContext context, IServiceProvider serviceProvider, Func<UpdateContext, Task> next) =>
            ((IUpdateHandler)serviceProvider.GetRequiredService(_updateHandlerType)).HandleAsync(context, next);
    }
}
