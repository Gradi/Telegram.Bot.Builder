using System;

namespace Telegram.Bot.Builder.Webhook
{
    /// <summary>
    /// Options for self-signed certificate.
    /// </summary>
    public class SelfSignedCertificateOptions
    {
        /// <summary>
        /// 2 letter country code.
        /// </summary>
        public string? CountryName { get; set; }

        /// <summary>
        /// State or Province Name (full name)
        /// </summary>
        public string? StateName { get; set; }

        /// <summary>
        /// Your locality (eg, city)
        /// </summary>
        public string? Locality { get; set; }

        public string? OrganizationUnitName { get; set; }

        public string? EmailAddress { get; set; }

        /// <summary>
        /// Certificate common name. If null then <see cref="WebhookOptions.PublicDomain"/>
        /// or <see cref="WebhookOptions.ListenUrl"/> will be used.
        /// </summary>
        public string? CommonName { get; set; }

        /// <summary>
        /// RSA bit size.
        /// By default, 2048
        /// </summary>
        public int RsaBitSize { get; set; } = 2048;

        /// <summary>
        /// Certificate validity time.
        /// By default, 1 year
        /// </summary>
        public TimeSpan ValidityTime { get; set; } = TimeSpan.FromDays(365);
    }
}
