using System.Text;
using NUnit.Framework;
using Telegram.Bot.Builder.Webhook.Extensions;

namespace Test.Telegram.Bot.Builder.Webhook.Tests.Extensions
{
    [TestFixture]
    public class TestStringBuilderExtensions
    {
        [Test]
        public void AppendIfAny()
        {
            var builder = new StringBuilder().AppendIfAny("@");
            Assert.That(builder.Length, Is.Zero);
            builder.Append(" ").AppendIfAny("@");
            Assert.That(builder.ToString(), Is.EqualTo(" @"));
            builder.AppendIfAny("@");
            Assert.That(builder.ToString(), Is.EqualTo(" @@"));
        }
    }
}
