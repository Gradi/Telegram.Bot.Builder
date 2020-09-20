using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Telegram.Bot.Builder.Hosting;
using Telegram.Bot.Builder.UpdateListening;
using Telegram.Bot.Builder.Webhook.Internals;

namespace Telegram.Bot.Builder.Webhook.Extensions
{
    public static class TelegramBotHostBuilderExtensions
    {
        /// <summary>
        /// Adds Webhook update listener.
        /// </summary>
        /// <remarks>
        /// You need to configure <see cref="WebhookOptions"/>.
        /// Additionally, you may need to configure <see cref="SelfSignedCertificateOptions"/>
        /// </remarks>
        public static ITelegramBotHostBuilder UseWebhookUpdateListener(this ITelegramBotHostBuilder builder, Action<WebhookOptions> webhookConfig)
        {
            return builder.ConfigureServices(sc =>
            {
                sc.AddOptions<WebhookOptions>().Configure(webhookConfig);
                sc.AddOptions<SelfSignedCertificateOptions>();

                sc.AddSingleton<OptionsValidator>();
                sc.AddSingleton<IValidateOptions<WebhookOptions>>(sp => sp.GetRequiredService<OptionsValidator>());
                sc.AddSingleton<IValidateOptions<SelfSignedCertificateOptions>>(sp => sp.GetRequiredService<OptionsValidator>());

                sc.AddSingleton<WebhookErrorChecker>();
                sc.TryAddSingleton<ICertificateFactory, BouncyCastleCertificateFactory>();
                sc.AddSingleton<IUpdateListener, WebhookUpdateListener>();
            });
        }
    }
}
