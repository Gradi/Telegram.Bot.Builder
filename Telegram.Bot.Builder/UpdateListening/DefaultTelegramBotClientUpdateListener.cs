using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Telegram.Bot.Builder.UpdateListening
{
    internal class DefaultTelegramBotClientUpdateListener : IUpdateListener
    {
        private readonly ITelegramBotClient _telegramBotClient;
        private readonly ILogger _logger;

        public event OnUpdate? OnUpdate;

        public DefaultTelegramBotClientUpdateListener
            (
                ITelegramBotClient telegramBotClient,
                ILogger<DefaultTelegramBotClientUpdateListener> logger
            )
        {
            _telegramBotClient = telegramBotClient;
            _logger = logger;
        }

        public Task StartAsync()
        {
            _telegramBotClient.OnUpdate += InnerOnUpdate;
            _telegramBotClient.OnReceiveError += InnerOnReceiveError;
            _telegramBotClient.OnReceiveGeneralError += InnerOnReceiveGeneralError;
            _telegramBotClient.StartReceiving();
            return Task.CompletedTask;
        }

        public Task StopAsync()
        {
            _telegramBotClient.StopReceiving();
            _telegramBotClient.OnUpdate -= InnerOnUpdate;
            _telegramBotClient.OnReceiveError -= InnerOnReceiveError;
            _telegramBotClient.OnReceiveGeneralError -= InnerOnReceiveGeneralError;
            return Task.CompletedTask;
        }

        private void InnerOnUpdate(object sender, Telegram.Bot.Args.UpdateEventArgs args)
        {
            if (OnUpdate == null)
            {
                _logger.LogWarning("Got an update but no one is subscribed for event.");
                return;
            }

            try
            {
                OnUpdate?.Invoke(this, new UpdateEventArgs(args.Update));
            }
            catch(Exception exception)
            {
                _logger.LogError(exception, "Error occurred on update handling.");
            }
        }

        private void InnerOnReceiveError(object sender, Telegram.Bot.Args.ReceiveErrorEventArgs args) =>
            _logger.LogError(args.ApiRequestException, "Receive error occurred.");

        private void InnerOnReceiveGeneralError(object sender, Telegram.Bot.Args.ReceiveGeneralErrorEventArgs args) =>
            _logger.LogError(args.Exception, "General receive error occurred.");
    }
}
