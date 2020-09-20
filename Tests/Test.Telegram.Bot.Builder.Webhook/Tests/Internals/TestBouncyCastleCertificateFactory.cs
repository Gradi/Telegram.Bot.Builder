using System.IO;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using Telegram.Bot.Builder.Webhook;
using Telegram.Bot.Builder.Webhook.Internals;

namespace Test.Telegram.Bot.Builder.Webhook.Tests.Internals
{
    [TestFixture]
    public class TestBouncyCastleCertificateFactory
    {
        private ILogger<BouncyCastleCertificateFactory> _logger = null!;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _logger = new TestContextLogger<BouncyCastleCertificateFactory>();
        }

        /**
         * I had some weird bug when generation of certificate failed with weird error.
         * So, let's run it several times.
         **/
        [Test]
        [Repeat(10)]
        public void DoesNotThrow()
        {
            var factory = new BouncyCastleCertificateFactory(_logger, new OptionsWrapper<SelfSignedCertificateOptions>(new SelfSignedCertificateOptions()),
                new OptionsWrapper<WebhookOptions>(new WebhookOptions()));

            using var reader = new StreamReader(factory.GetPublicCertificate(), Encoding.UTF8);
            var certificate = reader.ReadToEnd();
            Assert.That(certificate.StartsWith("-----BEGIN CERTIFICATE-----"), Is.True);
            Assert.That(certificate.Contains("-----END CERTIFICATE-----"), Is.True);

            Assert.That(factory.GetPrivateCertificate(), Is.Not.Null);
            Assert.That(factory.GetPrivateCertificate().PublicKey, Is.Not.Null);
            Assert.That(factory.GetPrivateCertificate().PrivateKey, Is.Not.Null);
        }
    }
}
