using System.Collections.Generic;
using Telegram.Bot.Builder.Controllers.Descriptors;

namespace Telegram.Bot.Builder.Controllers.Binders
{
    internal interface IArgumentBinder
    {
        object[] Bind(ActionDescriptor action, IEnumerable<BotCommand.Argument> arguments);
    }
}
