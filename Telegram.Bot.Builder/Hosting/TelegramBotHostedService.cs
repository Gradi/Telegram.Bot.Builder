using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Builder.Extensions;
using Telegram.Bot.Builder.UpdateHandling;
using Telegram.Bot.Builder.UpdateListening;

namespace Telegram.Bot.Builder.Hosting
{
    internal class TelegramBotHostedService : IHostedService
    {
        private readonly ILogger _logger;
        private readonly IStartupDescriptor _startup;
        private readonly IUpdateListener _updateListener;
        private readonly IServiceProvider _serviceProvider;
        private readonly ITelegramBotClient _telegramBotClient;

        private IAdvancedUpdateHandler _updateHandler;
        private bool _isRunning;

        private long _runningUpdates;

        public TelegramBotHostedService
            (
                ILogger<TelegramBotHostedService> logger,
                IStartupDescriptor startupDescriptor,
                IUpdateListener updateListener,
                IServiceProvider serviceProvider,
                ITelegramBotClient telegramBotClient
            )
        {
            _logger = logger;
            _startup = startupDescriptor;
            _updateListener = updateListener;
            _serviceProvider = serviceProvider;
            _telegramBotClient = telegramBotClient;

            _updateHandler = new EmptyUpdateHandler();
            _isRunning = false;
            _runningUpdates = 0;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var stopwatch = Stopwatch.StartNew();
            _logger.LogInformation("Starting bot application...");

            try
            {
                using var scope = _serviceProvider.CreateScope();
                var appBuilder = new ApplicationBuilder();
                _startup.Configure(appBuilder, scope.ServiceProvider);
                _updateHandler = appBuilder.Build();

                _updateListener.OnUpdate += this.OnUpdate;
                await _updateListener.StartAsync();

                Thread.MemoryBarrier();
                _isRunning = true;
                _logger.LogInformation("Application started after {ElapsedStartTime}", stopwatch.Elapsed);
            }
            catch(Exception exception)
            {
                _logger.LogCritical(exception, "Error occurred on bot application startup.");
                throw;
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_isRunning)
            {
                try
                {
                    var stopwatch = Stopwatch.StartNew();
                    _logger.LogInformation("Stopping bot application...");

                    await _updateListener.StopAsync();
                    _updateListener.OnUpdate -= this.OnUpdate;

                    await WaitForRunningUpdates();

                    Thread.MemoryBarrier();
                    _isRunning = false;
                    _logger.LogInformation("Bot application stopped after {ElapsedStopTime}", stopwatch.Elapsed);
                }
                catch(Exception exception)
                {
                    _logger.LogCritical(exception, "Error occurred on stopping bot application.");
                    throw;
                }
            }
        }

        private void OnUpdate(object sender, Telegram.Bot.Builder.UpdateListening.UpdateEventArgs args)
        {
            Task.Run(async () =>
            {
                var stopwatch = Stopwatch.StartNew();
                try
                {
                    Interlocked.Increment(ref _runningUpdates);

                    _logger.LogTrace("Handling update...");
                    using(var scope = _serviceProvider.CreateScope())
                    {
                        await _updateHandler.HandleAsync(CreateContext(args), scope.ServiceProvider, _ => Task.CompletedTask);
                    }
                    _logger.LogTrace("Handled update in \"{UpdateHandleTime}\"", stopwatch.Elapsed);
                }
                catch(Exception exception)
                {
                    _logger.LogError(exception, "Error occurred on update handling after \"{UpdateHandleTime}\"", stopwatch.Elapsed);
                }
                finally
                {
                    Interlocked.Decrement(ref _runningUpdates);
                }
            });
        }

        private async Task WaitForRunningUpdates()
        {
            while (Interlocked.Read(ref _runningUpdates) != 0)
            {
                _logger.LogTrace("Waiting for currently running update handlers to finish...");
                await Task.Delay(TimeSpan.FromSeconds(1));
            }
        }

        private UpdateContext CreateContext(Telegram.Bot.Builder.UpdateListening.UpdateEventArgs args)
        {
            BotCommand.TryParse(args.Update.GetMessageTextData(), out var command);
            return new UpdateContext(_telegramBotClient, args.Update, command);
        }
    }
}
