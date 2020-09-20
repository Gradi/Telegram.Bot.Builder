using System.Text.RegularExpressions;

namespace Telegram.Bot.Builder.Webhook.Extensions
{
    internal static class WebhookOptionsExtensions
    {
        public static string GetPublicDomain(this WebhookOptions options) =>
            options.PublicDomain ?? options.ListenUrl.ToString();

        public static string GetWebhookUrl(this WebhookOptions options) =>
            $"https://{GetPublicDomain(options)}{options.BotPath}";

        public static string GetCommonName(this WebhookOptions options)
        {
            if (options.PublicDomain == null)
                return options.ListenUrl.Address.ToString();

            const string domainWithPortRegex = @"\:[0-9]+$";

            return Regex.Replace(options.PublicDomain, domainWithPortRegex, string.Empty);
        }
    }
}
