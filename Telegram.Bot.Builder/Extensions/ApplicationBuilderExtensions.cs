using System;
using System.Threading.Tasks;
using Telegram.Bot.Builder.Controllers;
using Telegram.Bot.Builder.Hosting;
using Telegram.Bot.Builder.UpdateHandling;

namespace Telegram.Bot.Builder.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder Use(this IApplicationBuilder builder, Func<UpdateContext, Task> handler)
        {
            return builder.Use((ctx, _) => handler(ctx));
        }

        public static IApplicationBuilder Use(this IApplicationBuilder builder, Action<UpdateContext> handler)
        {
            return builder.Use((ctx, _) =>
            {
                handler(ctx);
                return Task.CompletedTask;
            });
        }

        public static IApplicationBuilder Use(this IApplicationBuilder builder, Func<UpdateContext, Func<UpdateContext, Task>, Task> handler)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            return builder.Use(new FuncUpdateHandler(handler));
        }

        public static IApplicationBuilder UseControllers(this IApplicationBuilder builder)
        {
            builder.Use<ControllersUpdateHandler>();
            return builder;
        }
    }
}
