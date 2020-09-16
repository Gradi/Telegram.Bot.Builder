using System;
using NUnit.Framework;
using Telegram.Bot.Builder.Extensions;

namespace Test.Telegram.Bot.Builder.Tests.Extensions
{
    [TestFixture]
    public class TypeExtensions
    {
        [TestCase(typeof(object), typeof(object), ExpectedResult = true)]
        [TestCase(typeof(TypeExtensions), typeof(object), ExpectedResult = true)]
        [TestCase(typeof(object), typeof(DateTime), ExpectedResult = false)]
        [TestCase(typeof(DateTime), typeof(TimeSpan), ExpectedResult = false)]
        public bool IsValidFinalImplementationOf(Type left, Type right) => left.IsValidFinalImplementationOf(right);
    }
}
