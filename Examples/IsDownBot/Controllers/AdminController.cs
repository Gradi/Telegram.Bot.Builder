using System.Threading.Tasks;
using Telegram.Bot.Builder.Controllers;
using Telegram.Bot.Builder.Controllers.Attributes;

namespace IsDownBot.Controllers
{
    public class AdminController : Controller
    {
        [DefaultCommand]
        public Task DefaultCommand()
        {
            var msg = UpdateContext.Update.Message;
            if (msg == null)
                return Task.CompletedTask;

            return Client.SendTextMessageAsync(msg.Chat.Id, "You're admin. Congratulations!", replyToMessageId: msg.MessageId);
        }

        public Task Ban(int userId = -1)
        {
            var msg = UpdateContext.Update.Message;
            var chat = msg?.Chat?.Id;

            if (chat == null)
                return Task.CompletedTask;

            if (msg!.ReplyToMessage != null)
            {
                return Client.SendTextMessageAsync(chat, $"User {msg.ReplyToMessage.From.Id}  from reply message is banned (not really).");
            }
            else if (userId != -1)
            {
                return Client.SendTextMessageAsync(chat, $"User {userId} is banned (not really).");
            }
            else
            {
                return Client.SendTextMessageAsync(chat, "Don't know whom to ban.");
            }
        }
    }
}
