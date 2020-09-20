using System;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Builder.Extensions;

namespace EchoBot
{
    public static class Program
    {
        public static void Main(string[] args) => CreateHostBuilder().Build().Run();

        private static IHostBuilder CreateHostBuilder()
        {
            return new HostBuilder()
                .UseDefaultServiceProvider(sp =>
                {
                    sp.ValidateScopes = true;
                    sp.ValidateOnBuild = true;
                })
                .UseConsoleLifetime()
                .ConfigureLogging(log =>
                {
                    log.ClearProviders();
                    log.SetMinimumLevel(LogLevel.Trace);
                    log.AddConsole();
                })
                .ConfigureTelegramBotHost(builder =>
                {
                    builder
                        .UseBotToken(GetBotToken())
                        .UseStartup<Startup>()
                        .UseDefaultTelegramBotClientUpdateListener();
                });
        }

        private static string GetBotToken()
        {
            const string envVar = "BOT_TOKEN";
            var token = Environment.GetEnvironmentVariable(envVar);
            if (string.IsNullOrWhiteSpace(token))
                throw new Exception($"Can't get bot token from environment variable. Did you forget to set \"{envVar}\"?");

            return token;
        }
    }
}
