using NUnit.Framework;
using Telegram.Bot.Builder.Extensions;

namespace Test.Telegram.Bot.Builder.Tests.Extensions
{
    [TestFixture]
    public class TestStringExtensions
    {
        [TestCase(null, null, ExpectedResult = null)]
        [TestCase(null, "", ExpectedResult = null)]
        [TestCase("", null, ExpectedResult = "")]
        [TestCase("", "", ExpectedResult = "")]
        [TestCase("Hello there", null, ExpectedResult = "Hello there")]
        [TestCase("Hello there", "there", ExpectedResult = "Hello ")]
        public string? TrimEnd(string? input, string? what) => input.TrimEnd(what);
    }
}
