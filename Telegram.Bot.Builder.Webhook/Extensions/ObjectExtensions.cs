using System.IO;
using System.Text;
using Org.BouncyCastle.OpenSsl;

namespace Telegram.Bot.Builder.Webhook.Extensions
{
    internal static class ObjectExtensions
    {
        public static string ToPemString(this object obj)
        {
            using var writer = new StringWriter();
            new PemWriter(writer).WriteObject(obj);
            return writer.ToString();
        }

        public static byte[] ToPemBytes(this object obj) => Encoding.UTF8.GetBytes(ToPemString(obj));
    }
}
