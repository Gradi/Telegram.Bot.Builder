using System.Text;

namespace Telegram.Bot.Builder.Webhook.Extensions
{
    internal static class StringBuilderExtensions
    {
        public static StringBuilder AppendIfAny(this StringBuilder builder, string value)
        {
            if (builder.Length != 0)
            {
                builder.Append(value);
            }
            return builder;
        }
    }
}
