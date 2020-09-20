using System;
using System.Threading.Tasks;
using IsDownBot.Extensions;
using IsDownBot.Services;
using Telegram.Bot.Builder.Controllers;
using Telegram.Bot.Builder.Controllers.Attributes;
using Telegram.Bot.Types.Enums;

namespace IsDownBot.Controllers
{
    public class IsDownController : Controller
    {
        private readonly IDownDetector _downDetector;

        public IsDownController(IDownDetector downDetector)
        {
            _downDetector = downDetector;
        }

        [DefaultCommand]
        public async Task CheckUrl(string url, int timeoutSeconds = 5)
        {
            var msg = UpdateContext.Update.Message;
            if (msg == null)
                return;

            if (!url.IsValidUri())
            {
                await Client.SendTextMessageAsync(msg.Chat.Id, $"\"{url}\" is not valid url.", replyToMessageId: msg.MessageId);
            }
            else
            {
                var status = await _downDetector.GetUrlStatus(url, TimeSpan.FromSeconds(Math.Max(timeoutSeconds, 0)));
                await Client.SendTextMessageAsync(
                    msg.Chat.Id,
                    $"URL status is:{Environment.NewLine}```{Environment.NewLine}{status}{Environment.NewLine}```",
                    parseMode: ParseMode.MarkdownV2);
            }
        }
    }
}
