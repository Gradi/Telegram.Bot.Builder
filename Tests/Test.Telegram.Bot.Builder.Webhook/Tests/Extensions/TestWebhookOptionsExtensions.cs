using System.Net;
using NUnit.Framework;
using Telegram.Bot.Builder.Webhook;
using Telegram.Bot.Builder.Webhook.Extensions;

namespace Test.Telegram.Bot.Builder.Webhook.Tests.Extensions
{
    [TestFixture]
    public class TestWebhookOptionsExtensions
    {
        [Test]
        public void GetPublicDomain()
        {
            var options = new WebhookOptions();
            options.ListenUrl = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8855);
            Assert.That(options.GetPublicDomain(), Is.EqualTo("127.0.0.1:8855"));
            options.PublicDomain = "domain.com";
            Assert.That(options.GetPublicDomain(), Is.EqualTo("domain.com"));
        }

        [Test]
        public void GetWebhookUrl()
        {
            var options = new WebhookOptions();
            options.PublicDomain = "domain.com";
            options.BotPath = "/botpath";

            Assert.That(options.GetWebhookUrl(), Is.EqualTo("https://domain.com/botpath"));
        }

        [Test]
        public void GetCommonNameFromListenUrl()
        {
            var options = new WebhookOptions
            {
                ListenUrl = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8500)
            };
            Assert.That(options.GetCommonName(), Is.EqualTo("127.0.0.1"));
        }

        [TestCase("domain", ExpectedResult = "domain")]
        [TestCase("www.domain.com", ExpectedResult = "www.domain.com")]
        [TestCase("www.domain.com:8443", ExpectedResult = "www.domain.com")]
        [TestCase("www.domain.com:65:8443", ExpectedResult = "www.domain.com:65")]
        public string GetCommonNameFromPublicDomain(string input)
        {
            return new WebhookOptions { PublicDomain = input }.GetCommonName();
        }
    }
}
