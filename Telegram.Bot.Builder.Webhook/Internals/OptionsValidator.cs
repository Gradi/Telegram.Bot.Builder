using System;
using System.Collections.Generic;
using Microsoft.Extensions.Options;

namespace Telegram.Bot.Builder.Webhook.Internals
{
    internal class OptionsValidator :
        IValidateOptions<WebhookOptions>,
        IValidateOptions<SelfSignedCertificateOptions>
    {
        ValidateOptionsResult IValidateOptions<WebhookOptions>.Validate(string name, WebhookOptions options)
        {
            var failures = new List<string>();

            if (options.ListenUrl == null)
                failures.Add($"{nameof(options.ListenUrl)} is null.");

            if (options.PublicDomain != null && string.IsNullOrWhiteSpace(options.PublicDomain))
                failures.Add($"{nameof(options.PublicDomain)}({options.PublicDomain}) is not null, but is empty or whitespace.");

            if (options.MaxConnections.HasValue && options.MaxConnections <= 0)
                failures.Add($"{nameof(options.MaxConnections)}({options.MaxConnections}) is not null and <= 0.");

            return failures.Count != 0 ?
                ValidateOptionsResult.Fail(failures) :
                ValidateOptionsResult.Success;
        }

        ValidateOptionsResult IValidateOptions<SelfSignedCertificateOptions>.Validate(string name, SelfSignedCertificateOptions options)
        {
            var failures = new List<string>();

            if (options.RsaBitSize <= 0)
                failures.Add($"{nameof(options.RsaBitSize)} ({options.RsaBitSize}) is <= 0.");

            if (options.ValidityTime <= TimeSpan.Zero)
                failures.Add($"{nameof(options.ValidityTime)}({options.ValidityTime}) is <= 0.");

            return failures.Count != 0 ?
                ValidateOptionsResult.Fail(failures) :
                ValidateOptionsResult.Success;
        }
    }
}
