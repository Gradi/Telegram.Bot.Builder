using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;

namespace Telegram.Bot.Builder.Webhook.Extensions
{
    internal static class X509NameExtensions
    {
        public static string Is(this DerObjectIdentifier name, string value) =>
            $"{X509Name.DefaultSymbols[name]}={value}";
    }
}
