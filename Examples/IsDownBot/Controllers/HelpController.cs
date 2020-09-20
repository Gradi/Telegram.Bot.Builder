using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Builder.Controllers;
using Telegram.Bot.Builder.Controllers.Attributes;
using Telegram.Bot.Builder.Controllers.Descriptors;
using Telegram.Bot.Types.Enums;

namespace IsDownBot.Controllers
{
    public class HelpController : Controller
    {
        private readonly IControllerDescriptorCollection _controllers;

        public HelpController(IControllerDescriptorCollection controllers)
        {
            _controllers = controllers;
        }

        [DefaultCommand]
        public Task Default()
        {
            var msg = UpdateContext.Update.Message;
            if (msg == null)
                return Task.CompletedTask;

            var builder = new StringBuilder();
            builder.AppendLine("```");
            foreach (var controller in _controllers)
            {
                FormatController(builder, controller);
                builder.AppendLine();
            }
            builder.AppendLine("```");

            return Client.SendTextMessageAsync(msg.Chat.Id, builder.ToString(),
                replyToMessageId: msg.MessageId, parseMode: ParseMode.MarkdownV2);
        }

        private void FormatController(StringBuilder builder, ControllerDescriptor controller)
        {
            builder.Append('/').AppendLine(controller.Name);
            foreach (var action in controller.Actions)
            {
                FormatAction(builder, action);
                builder.AppendLine();
            }
            if (controller.DefaultAction != null)
            {
                builder.Append("Default: ");
                FormatAction(builder, controller.DefaultAction);
            }
            builder.AppendLine();
        }

        private void FormatAction(StringBuilder builder, ActionDescriptor actionDescriptor)
        {
            if (!actionDescriptor.IsDefault)
            {
                builder.Append(actionDescriptor.Name).Append(" ");
            }

            builder
                .Append('(')
                .Append(string.Join(", ", actionDescriptor.Arguments.Select(FormatArgument)))
                .Append(')');
        }

        private string FormatArgument(ArgumentDescriptor argument)
        {
            var builder = new StringBuilder().Append(argument.Parameter.Name);

            if (argument.Parameter.HasDefaultValue)
                builder.Append("=").Append(argument.Parameter.DefaultValue);

            return builder.ToString();
        }
    }
}
