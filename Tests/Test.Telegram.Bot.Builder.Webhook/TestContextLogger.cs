using System;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace Test.Telegram.Bot.Builder.Webhook
{
    internal class TestContextLogger<T> : ILogger<T>
    {
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (TestContext.Out != null)
            {
                TestContext.Out.WriteLine($"[{DateTime.Now}, {logLevel}]: {formatter(state, exception)}");
            }
        }

        public bool IsEnabled(LogLevel logLevel) => true;

        public IDisposable BeginScope<TState>(TState state) => new Disposable();

        private class Disposable : IDisposable
        {
            public void Dispose() {}
        }
    }
}
