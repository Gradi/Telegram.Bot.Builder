using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace Telegram.Bot.Builder.Webhook
{
    /// <summary>
    /// Interface for generating self-signed certificate
    /// that will be used during application lifetime.
    /// You may add your implementation.
    /// </summary>
    public interface ICertificateFactory
    {
        /// <summary>
        /// Returns stream which contains PEM encoded X509 self-signed certificate with public key only.
        /// This certificate will be sent to Telegram API.
        /// </summary>
        Stream GetPublicCertificate();

        /// <summary>
        /// Returns X509 self-signed certificate with private key that will be used
        /// by Kestrel to handle https connections.
        /// </summary>
        X509Certificate2 GetPrivateCertificate();
    }
}
