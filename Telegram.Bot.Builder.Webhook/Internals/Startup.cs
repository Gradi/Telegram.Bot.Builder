using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Builder.Webhook.Internals.Middlewares;

namespace Telegram.Bot.Builder.Webhook.Internals
{
    internal class Startup
    {
        public void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<UpdateMiddleware>();
            serviceCollection.AddSingleton<PingMiddleware>();
            serviceCollection.AddSingleton<BadRequestResponderMiddleware>();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseMiddleware<UpdateMiddleware>();
            app.UseMiddleware<PingMiddleware>();
            app.UseMiddleware<BadRequestResponderMiddleware>();
        }
    }
}
