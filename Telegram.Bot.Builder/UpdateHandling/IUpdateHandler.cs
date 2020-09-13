using System;
using System.Threading.Tasks;

namespace Telegram.Bot.Builder.UpdateHandling
{
    public interface IUpdateHandler
    {
        Task HandleAsync(UpdateContext context, Func<UpdateContext, Task> next);
    }
}
