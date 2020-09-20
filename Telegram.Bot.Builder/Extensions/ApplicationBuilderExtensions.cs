using System;
using System.Threading.Tasks;
using Telegram.Bot.Builder.Controllers;
using Telegram.Bot.Builder.Hosting;
using Telegram.Bot.Builder.UpdateHandling;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Builder.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder Use(this IApplicationBuilder builder, UpdateType updateType,
            Action<UpdateContext> handler)
        {
            return builder.Use(updateType, (ctx, _) =>
            {
                handler(ctx);
                return Task.CompletedTask;
            });
        }

        public static IApplicationBuilder Use(this IApplicationBuilder builder, UpdateType updateType,
            Func<UpdateContext, Task> handler)
        {
            return builder.Use(updateType, (ctx, _) => handler(ctx));
        }

        public static IApplicationBuilder Use(this IApplicationBuilder builder, UpdateType updateType,
            Func<UpdateContext, Func<UpdateContext, Task>, Task> handler)
        {
            return builder.Use((ctx, next) => ctx.Update.Type == updateType ? handler(ctx, next) : next(ctx));
        }

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
