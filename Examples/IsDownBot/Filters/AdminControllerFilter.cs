using System.Threading.Tasks;
using IsDownBot.Controllers;
using IsDownBot.Extensions;
using IsDownBot.Services;
using Telegram.Bot.Builder.Controllers.Filters;

namespace IsDownBot.Filters
{
    public class AdminControllerFilter : IFilter
    {
        private readonly IUserService _userService;

        public AdminControllerFilter(IUserService userService)
        {
            _userService = userService;
        }

        public async Task OnActionInvocation(ControllerContext context)
        {
            var userId = context.UpdateContext.Update.GetUserId();

            var msg = context.UpdateContext.Update.Message ??
                      context.UpdateContext.Update.EditedMessage;

            if (!userId.HasValue ||
                (context.ControllerDescriptor.Type == typeof(AdminController) && !_userService.IsAdmin(userId.Value)))
            {
                if (msg != null)
                {
                    await context.UpdateContext.BotClient.SendTextMessageAsync(msg.Chat.Id, "You are not an admin. This incident will be reported.",
                        replyToMessageId: msg.MessageId);
                }
                context.IsCanceled = true;
            }
        }

        public Task PostActionInvocation(ControllerContext context) => Task.CompletedTask;
    }
}
