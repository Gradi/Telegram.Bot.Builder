using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Telegram.Bot.Builder;

namespace Test.Telegram.Bot.Builder.Tests
{
    [TestFixture]
    public class TestBotCommand
    {
        [TestCase(null)]
        [TestCase("")]
        [TestCase("    ")]
        [TestCase("\n\t")]
        [TestCase("\n\t  ")]
        [TestCase("\\")]
        [TestCase("/")]
        [TestCase("\"")]
        [TestCase("\"\"")]

        [TestCase("/")]
        [TestCase("/@")]
        [TestCase("/  ")]
        [TestCase("/\n")]
        [TestCase("/@")]
        [TestCase("/#")]
        [TestCase("/%")]

        [TestCase("/co\\mmand")]
        [TestCase("/command =")]
        [TestCase("/command val\"ue")]
        [TestCase("/command name=\"   ")]
        [TestCase("/command name=\"")]
        public void BadInputs(string? input)
        {
            Assert.That(BotCommand.TryParse(input, out var command), Is.False);
            Assert.That(command, Is.Null);
        }

        [TestCase("/test", ExpectedResult = "test")]
        [TestCase("/t", ExpectedResult = "t")]
        [TestCase("/t123", ExpectedResult = "t123")]
        [TestCase("/123", ExpectedResult = "123")]
        [TestCase("/123test", ExpectedResult = "123test")]
        [TestCase("/1", ExpectedResult = "1")]

        [TestCase("/test@", ExpectedResult = "test")]
        [TestCase("/t@", ExpectedResult = "t")]
        [TestCase("/t123@", ExpectedResult = "t123")]
        [TestCase("/123@", ExpectedResult = "123")]
        [TestCase("/123test@", ExpectedResult = "123test")]
        [TestCase("/1@", ExpectedResult = "1")]

        [TestCase("/test@  ", ExpectedResult = "test")]
        [TestCase("/t@   ", ExpectedResult = "t")]
        [TestCase("/t123@  ", ExpectedResult = "t123")]
        [TestCase("/123@   ", ExpectedResult = "123")]
        [TestCase("/123test@  ", ExpectedResult = "123test")]
        [TestCase("/1@   ", ExpectedResult = "1")]

        [TestCase("/test   ", ExpectedResult = "test")]
        [TestCase("/t   ", ExpectedResult = "t")]
        [TestCase("/t123   ", ExpectedResult = "t123")]
        [TestCase("/123   ", ExpectedResult = "123")]
        [TestCase("/123test  ", ExpectedResult = "123test")]
        [TestCase("/1   ", ExpectedResult = "1")]
        public string CommandName(string input)
        {
            Assert.That(BotCommand.TryParse(input, out var command), Is.True);
            Assert.That(command, Is.Not.Null);
            Assert.That(command.BotName, Is.Null);
            Assert.That(command.Arguments.Count, Is.Zero);
            return command.Command;
        }

        [TestCase("/command@bot", ExpectedResult = "bot")]
        [TestCase("/command@bot   ", ExpectedResult = "bot")]
        [TestCase("/command@bot123", ExpectedResult = "bot123")]
        [TestCase("/command@1231bot123", ExpectedResult = "1231bot123")]
        [TestCase("/command@bot123   ", ExpectedResult = "bot123")]
        [TestCase("/command@1231bot123\n  ", ExpectedResult = "1231bot123")]
        [TestCase("/command@a", ExpectedResult = "a")]
        [TestCase("/command@22", ExpectedResult = "22")]
        [TestCase("/command@6", ExpectedResult = "6")]
        public string BotName(string input)
        {
            Assert.That(BotCommand.TryParse(input, out var command), Is.True);
            Assert.That(command, Is.Not.Null);
            Assert.That(command.Command, Is.EqualTo("command"));
            Assert.That(command.BotName, Is.Not.Null);
            Assert.That(command.Arguments.Count, Is.Zero);
            return command.BotName!;
        }

        [TestCase("/command val1", false, ExpectedResult = "val1")]
        [TestCase("/command val1   ", false, ExpectedResult = "val1")]
        [TestCase("/command@bot val1", true, ExpectedResult = "val1")]
        [TestCase("/command@bot val1   ", true, ExpectedResult = "val1")]
        public string SingleArgument(string input, bool withBotName)
        {
            Assert.That(BotCommand.TryParse(input, out var command), Is.True);
            Assert.That(command, Is.Not.Null);
            Assert.That(command.Command, Is.EqualTo("command"));
            if (withBotName) Assert.That(command.BotName, Is.EqualTo("bot"));
            else Assert.That(command.BotName, Is.Null);

            Assert.That(command.Arguments.Count, Is.EqualTo(1));
            Assert.That(command.Arguments.Single().Name, Is.Null);
            return command.Arguments.Single().Value;
        }

        [Test]
        public void SeveralUnnamedArguments()
        {
            const string input = "/command val1 val2 val3 \"value with space\" val4";

            Assert.That(BotCommand.TryParse(input, out var command), Is.True);
            Assert.That(command, Is.Not.Null);

            Assert.That(command.Command, Is.EqualTo("command"));
            Assert.That(command.BotName, Is.Null);
            Assert.That(command.Arguments.Select(a => a.Value), Is.EqualTo(new [] {"val1", "val2", "val3", "value with space", "val4"}).AsCollection);
        }

        [Test]
        public void NamedArguments()
        {
            const string input = "/command name1=value2   \n name2=$%asd@@dsd2val%u2 name3=\"value @#*&^ with 123\\\" space\" name4=\"\"";

            Assert.That(BotCommand.TryParse(input, out var command), Is.True);
            Assert.That(command, Is.Not.Null);
            Assert.That(command.Command, Is.EqualTo("command"));
            Assert.That(command.BotName, Is.Null);
            Assert.That(command.Arguments.Count, Is.EqualTo(4));
            Assert.That(command.Arguments.Select(a => a.Name), Is.EqualTo(new [] {"name1", "name2", "name3", "name4"}).AsCollection);
            Assert.That(command.Arguments.Select(a => a.Value), Is.EqualTo(new [] {"value2", "$%asd@@dsd2val%u2", "value @#*&^ with 123\" space", ""}));
        }

        [Test]
        public void RandomTests(
            [Random(1, 22, 3, Distinct = true)] int commandLength,
            [Values]bool withBotName,
            [Range(0, 11)] int argumentCount)
        {
            var commandName = TestContext.CurrentContext.Random.GetString(commandLength);
            string? botName = null;
            if (withBotName)
            {
                botName = TestContext.CurrentContext.Random.GetString(22);
            }

            var builder = new StringBuilder();
            builder.Append('/').Append(commandName);
            if (withBotName)
                builder.Append('@').Append(botName);
            var arguments = new List<BotCommand.Argument>();
            for(int i = 0; i < argumentCount; ++i)
            {
                var value = TestContext.CurrentContext.Random.GetString();
                string? name = null;
                if (TestContext.CurrentContext.Random.NextBool())
                    name = TestContext.CurrentContext.Random.GetString();

                builder.Append(' ').Append(name).Append(name != null ? "=" : string.Empty).Append(value);
                arguments.Add(new BotCommand.Argument(value, name));
            }

            TestContext.Out.WriteLine($"Input was: {builder}");
            Assert.That(BotCommand.TryParse(builder.ToString(), out var command), Is.True);
            Assert.That(command, Is.Not.Null);
            Assert.That(command.Command, Is.EqualTo(commandName));
            Assert.That(command.BotName, Is.EqualTo(botName));
            Assert.That(command.Arguments.Count, Is.EqualTo(argumentCount));

            Assert.That(command.Arguments.Select(a => a.Name), Is.EqualTo(arguments.Select(a => a.Name)).AsCollection);
            Assert.That(command.Arguments.Select(a => a.Value), Is.EqualTo(arguments.Select(a => a.Value)).AsCollection);
        }
    }
}
