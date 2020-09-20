using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Telegram.Bot.Builder.Webhook.Internals.Middlewares
{
    internal class PingMiddleware : IMiddleware
    {
        private readonly PathString _pingPath;

        public PingMiddleware(WebhookOptions webhookOptions)
        {
            _pingPath = webhookOptions.PingPath;
        }

        public Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var req = context.Request;
            if (req.Path == _pingPath)
            {
                context.Response.StatusCode = (int)HttpStatusCode.OK;
                return context.Response.WriteAsync("Ok.", Encoding.UTF8);
            }
            else
            {
                return next(context);
            }
        }
    }
}
