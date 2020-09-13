using System.Collections.Generic;
using System.Text;

namespace Telegram.Bot.Builder
{
    public class BotCommand
    {
        public string Command { get; }

        public string? BotName { get; }

        public IReadOnlyCollection<Argument> Arguments { get; }

        public BotCommand
            (
                string command,
                string? botName,
                IReadOnlyCollection<Argument> arguments
            )
        {
            Command = command;
            BotName = botName;
            Arguments = arguments;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append('/').Append(Command);
            if (BotName != null)
            {
                builder.Append('@').Append(BotName);
            }

            foreach (var argument in Arguments)
            {
                builder.Append(' ').Append(argument);
            }
            return builder.ToString();
        }

        public static bool TryParse(string? command, out BotCommand botCommand)
        {
#pragma warning disable 8601
            botCommand = BotCommandParser.Parse(command);
#pragma warning restore 8601
            return botCommand != null;
        }

        public readonly struct Argument
        {
            public readonly string Value;
            public readonly string? Name;

            public bool IsNamed => Name != null;

            public Argument(string value, string? name)
            {
                Value = value;
                Name = name;
            }

            public override string ToString() =>
                IsNamed ? $"{Name}=\"{Value}\"" : $"\"{Value}\"";
        }
    }
}
