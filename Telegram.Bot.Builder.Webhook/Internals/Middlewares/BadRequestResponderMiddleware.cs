using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Telegram.Bot.Builder.Webhook.Internals.Middlewares
{
    internal class BadRequestResponderMiddleware : IMiddleware
    {
        private readonly ILogger _logger;

        public BadRequestResponderMiddleware(ILogger<BadRequestResponderMiddleware> logger)
        {
            _logger = logger;
        }

        public Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            _logger.LogTrace("Got an unhandled request.");
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Task.CompletedTask;
        }
    }
}
