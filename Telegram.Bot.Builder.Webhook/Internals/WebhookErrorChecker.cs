using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Telegram.Bot.Builder.Webhook.Internals
{
    internal class WebhookErrorChecker : IDisposable
    {
        private readonly ILogger _logger;
        private readonly ITelegramBotClient _botClient;

        private System.Timers.Timer? _timer;

        public WebhookErrorChecker
            (
                ILogger<WebhookErrorChecker> logger,
                ITelegramBotClient telegramBotClient,
                IOptions<WebhookOptions> webhookOptions
            )
        {
            _logger = logger;
            _botClient = telegramBotClient;

            if (webhookOptions.Value.SetWebhook && webhookOptions.Value.WebhookCheckTime >= TimeSpan.Zero)
            {
                _timer = new System.Timers.Timer(webhookOptions.Value.WebhookCheckTime.TotalMilliseconds);
                _timer.Elapsed += OnTimer;
                _timer.AutoReset = true;
                _timer.Start();
                _logger.LogWarning("Webhook checking timer is activated. Interval is {TimerInterval}",
                    webhookOptions.Value.WebhookCheckTime);
            }
            else
            {
                _logger.LogWarning("Webhook checking timer is deactived.");
            }
        }

        public void Dispose()
        {
            if (_timer != null)
            {
                _timer.Stop();
                _timer.Elapsed -= OnTimer;
                _timer = null;
            }
        }

        private void OnTimer(object sender, System.Timers.ElapsedEventArgs args)
        {
            try
            {
                var webhook = _botClient.GetWebhookInfoAsync().GetAwaiter().GetResult();

                var failures = new List<string>();

                if (string.IsNullOrWhiteSpace(webhook.Url))
                    failures.Add("Webhook is missing url.");

                if (!webhook.HasCustomCertificate)
                    failures.Add("Webhook doesn't have custom certificate.");

                if (webhook.LastErrorDate != default || !string.IsNullOrWhiteSpace(webhook.LastErrorMessage))
                    failures.Add($"Webhook has last error message at \"{webhook.LastErrorDate}\": {webhook.LastErrorMessage}");

                if (failures.Count != 0)
                {
                    _logger.LogWarning("Webhook \"{Webhook}\" contains errors: \"{Errors}\"",
                        webhook, failures);
                }
                else
                {
                    _logger.LogTrace("Webhook \"{Webhook}\" is good.", webhook);
                }
            }
            catch(Exception exception)
            {
                _logger.LogError(exception, "Error on webhook verification.");
            }
        }
    }
}
