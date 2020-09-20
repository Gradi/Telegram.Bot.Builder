using System;
using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Builder.Webhook
{
    /// <summary>
    /// Options for Webhook.
    /// </summary>
    public class WebhookOptions
    {
        /// <summary>
        /// Listen url, e.g. http://*:5000
        /// </summary>
        public IPEndPoint ListenUrl { get; set; } = new IPEndPoint(IPAddress.Any, 5000);

        /// <summary>
        /// Path under which application will listen for updates.
        /// By default a random string.
        /// </summary>
        public PathString BotPath { get; set; } = $"/{Guid.NewGuid():N}{Guid.NewGuid():N}";

        /// <summary>
        /// Path which will always returns 200 with 'Ok' content.
        /// By default "/ping"
        /// </summary>
        public PathString PingPath { get; set; } = "/ping";

        /// <summary>
        /// Set to true to call 'setWebhook' with current options.
        /// Will auto generate self-signed certificate and Kestel will be configured to use that certificate.
        /// </summary>
        /// <remarks>
        ///     You may also configure <see cref="SelfSignedCertificateOptions"/> options.
        /// </remarks>
        public bool SetWebhook { get; set; }

        /// <summary>
        /// A public domain (e.g. yourbot.com ) under which this application is accessible.
        /// It will be used for setting webhook and self-signed certificate generation.
        /// If null, then <see cref="ListenUrl"/> will be used for that.
        /// </summary>
        /// <remarks>
        ///     <see cref="PingPath"/> will be automatically appended to this domain.
        /// </remarks>
        public string? PublicDomain { get; set; }

        /// <summary>
        /// Parameter for 'setWebhook'.
        /// Maximum allowed number of simultaneous connections.
        /// </summary>
        public int? MaxConnections { get; set; }

        /// <summary>
        /// Parameter for 'setWebhook'
        /// Array of allowed updates. Set to null to allow all updates.
        /// </summary>
        public UpdateType[]? AllowedUpdates { get; set; }

        /// <summary>
        /// If <see cref="SetWebhook"/> is true then this is
        /// periodic time to call 'getWebhookInfo' and log any errors.
        /// Values <= 0 cancels periodic check.
        /// </summary>
        public TimeSpan WebhookCheckTime { get; set;} = TimeSpan.FromMinutes(5);

        /// <summary>
        /// Action to tune the web host configuration.
        /// </summary>
        public Action<IWebHostBuilder>? ConfigureWebHost { get; set; }
    }
}
