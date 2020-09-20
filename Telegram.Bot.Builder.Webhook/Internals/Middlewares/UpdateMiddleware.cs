using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Telegram.Bot.Types;

namespace Telegram.Bot.Builder.Webhook.Internals.Middlewares
{
    internal class UpdateMiddleware : IMiddleware
    {
        private readonly ILogger _logger;
        private readonly PathString _botPath;
        private readonly IUpdateAccepter _updateAccepter;

        public UpdateMiddleware
            (
                ILogger<UpdateMiddleware> logger,
                WebhookOptions webhookOptions,
                IUpdateAccepter updateAccepter
            )
        {
            _logger = logger;
            _botPath = webhookOptions.BotPath;
            _updateAccepter = updateAccepter;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var req = context.Request;
            if (IsUpdateRequest(req))
            {
                var update = await TryReadUpdate(req);
                if (update != null)
                {
                    _updateAccepter.Accept(update);
                    context.Response.StatusCode = (int)HttpStatusCode.NoContent;
                }
            }
            else
            {
                await next(context);
            }
        }

        private bool IsUpdateRequest(HttpRequest request)
        {
            return string.Equals(request.Method, "POST", StringComparison.InvariantCulture) &&
                   request.Path == _botPath &&
                   request.ContentLength.HasValue && request.ContentLength.Value > 0;
        }

        private async Task<Update?> TryReadUpdate(HttpRequest request)
        {
            string? body = null;
            try
            {
                using var reader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true);
                body = await reader.ReadToEndAsync();

                return JsonConvert.DeserializeObject<Update>(body);
            }
            catch(Exception exception)
            {
                _logger.LogError(exception, "Error on reading update. Body was: {Body}", body);
                return null;
            }
        }
    }
}
