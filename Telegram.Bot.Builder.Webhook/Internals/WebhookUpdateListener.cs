using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot.Builder.UpdateListening;
using Telegram.Bot.Builder.Webhook.Extensions;
using Telegram.Bot.Types;

namespace Telegram.Bot.Builder.Webhook.Internals
{
    internal class WebhookUpdateListener : IUpdateListener, IUpdateAccepter
    {
        private readonly ILogger _logger;
        private readonly WebhookOptions _webhookOptions;
        private readonly IEnumerable<ILoggerProvider> _loggerProviders;
        private readonly LogLevel _minimumLogLevel;
        private readonly ITelegramBotClient _botClient;
        private readonly ICertificateFactory _certificateFactory;

        private IHost? _host;

        public event OnUpdate? OnUpdate;

        public WebhookUpdateListener
            (
                ILogger<WebhookUpdateListener> logger,
                IOptions<WebhookOptions> webhookOptions,
                IEnumerable<ILoggerProvider> loggerProviders,
                IOptions<LoggerFilterOptions> loggerFilterOptions,
                ITelegramBotClient telegramBotClient,
                ICertificateFactory certificateFactory,
                WebhookErrorChecker _
            )
        {
            _logger = logger;
            _webhookOptions = webhookOptions.Value;
            _loggerProviders = loggerProviders;
            _minimumLogLevel = loggerFilterOptions.Value.MinLevel;
            _botClient = telegramBotClient;
            _certificateFactory = certificateFactory;
        }

        public async Task StartAsync()
        {
            if (_host != null) throw new InvalidOperationException("Can't start again. Already listening for updates.");

            _host = CreateHostBuilder().Build();
            await _host.StartAsync();
            await ConfigureWebhook();
        }

        public async Task StopAsync()
        {
            if (_host == null)
                return;

            await DisableWebhook();
            await _host.StopAsync();
            _host.Dispose();
            _host = null;
        }

        public void Accept(Update update)
        {
            if (OnUpdate == null)
            {
                _logger.LogWarning("Got an update, but no one is listening for updates.");
                return;
            }

            try
            {
                OnUpdate(this, new Telegram.Bot.Builder.UpdateListening.UpdateEventArgs(update));
            }
            catch(Exception exception)
            {
                _logger.LogError(exception, "Error on update handling.");
            }
        }

        private IHostBuilder CreateHostBuilder()
        {
            return new HostBuilder()
                .UseDefaultServiceProvider(sp =>
                {
                    sp.ValidateScopes = true;
                    sp.ValidateOnBuild = true;
                })
                .ConfigureLogging(log =>
                {
                    log.ClearProviders();
                    log.SetMinimumLevel(_minimumLogLevel);
                    foreach (var loggerProvider in _loggerProviders)
                        log.AddProvider(loggerProvider);
                })
                .ConfigureWebHost(builder =>
                {
                    builder
                        .UseStartup<Startup>()
                        .UseKestrel(opt =>
                        {
                            opt.Listen(_webhookOptions.ListenUrl, opt =>
                            {
                                if (_webhookOptions.SetWebhook)
                                {
                                    opt.UseHttps(http =>
                                    {
                                        http.ServerCertificate = _certificateFactory.GetPrivateCertificate();
                                    });
                                }
                            });
                        });
                    _webhookOptions.ConfigureWebHost?.Invoke(builder);
                })
                .ConfigureServices(sc =>
                {
                    sc.AddSingleton<IUpdateAccepter>(_ => this);
                    sc.AddSingleton<WebhookOptions>(_ => _webhookOptions);
                });
        }

        private async Task ConfigureWebhook()
        {
            if (!_webhookOptions.SetWebhook)
            {
                _logger.LogWarning("Skipping webhook setup.");
                return;
            }
            _logger.LogWarning("Setting up webhook.");

            _logger.LogWarning("Removing any previous webhook.");
            await _botClient.DeleteWebhookAsync();

            using var certificate = _certificateFactory.GetPublicCertificate();
            var url = _webhookOptions.GetWebhookUrl();
            _logger.LogWarning("Setting webhook options. Url: \"{Url}\", Max connections: \"{MaxConnections}\", Allowed updates: \"{AllowedUpdates}\"",
                url, _webhookOptions.MaxConnections, _webhookOptions.AllowedUpdates);
            await _botClient.SetWebhookAsync
                (
                    url,
                    certificate,
                    _webhookOptions.MaxConnections.GetValueOrDefault(),
                    _webhookOptions.AllowedUpdates
                );
        }

        private async Task DisableWebhook()
        {
            if (_webhookOptions.SetWebhook)
            {
                _logger.LogWarning("Removing webhook.");
                await _botClient.DeleteWebhookAsync();
            }
        }
    }
}
