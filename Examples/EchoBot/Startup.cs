using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Builder.Extensions;
using Telegram.Bot.Builder.Hosting;
using Telegram.Bot.Types.Enums;

namespace EchoBot
{
    public class Startup : IStartup
    {
        public void ConfigureServices(IServiceCollection services) {}

        public void Configure(IApplicationBuilder app)
        {
            app
                .Use(UpdateType.Message, async ctx =>
                {
                    var msg = ctx.Update.Message;
                    await ctx.BotClient.SendTextMessageAsync(msg.Chat.Id, $"Hi there! You message is \"{msg.Text}\"",
                        replyToMessageId: msg.MessageId);
                })
                .Use(UpdateType.EditedMessage, async ctx =>
                {
                    var msg = ctx.Update.EditedMessage;
                    await ctx.BotClient.SendTextMessageAsync(msg.Chat.Id, "You've edited this message!",
                        replyToMessageId: msg.MessageId);
                });
        }
    }
}
