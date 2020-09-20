using System.Text;
using System.Threading.Tasks;
using IsDownBot.Services;
using Telegram.Bot.Builder.Controllers;
using Telegram.Bot.Builder.Controllers.Attributes;
using Telegram.Bot.Types.Enums;

namespace IsDownBot.Controllers
{
    public class WhoamiController : Controller
    {
        private readonly IUserService _userService;

        public WhoamiController(IUserService userService)
        {
            _userService = userService;
        }

        [DefaultCommand]
        public Task Default()
        {
            var msg = UpdateContext.Update.Message;
            if (msg == null)
                return Task.CompletedTask;

            var result = new StringBuilder()
                .AppendLine("```")
                .Append("Message id..........: ").AppendLine(msg.MessageId.ToString())
                .Append("Char id.............: ").AppendLine(msg.Chat.Id.ToString())
                .Append("User id.............: ").AppendLine(msg.From.Id.ToString())
                .Append("User name...........: ").AppendLine(msg.From.Username)
                .Append("User first name.....: ").AppendLine(msg.From.FirstName)
                .Append("User last name......: ").AppendLine(msg.From.LastName)
                .Append("Is admin............: ").AppendLine(_userService.IsAdmin(msg.From.Id).ToString())
                .AppendLine("```")
                .ToString();

            return Client.SendTextMessageAsync(msg.Chat.Id, result, replyToMessageId: msg.MessageId,
                parseMode: ParseMode.MarkdownV2);
        }
    }
}
