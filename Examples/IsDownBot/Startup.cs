using System;
using System.Collections.Generic;
using System.Linq;
using IsDownBot.Extensions;
using IsDownBot.Filters;
using IsDownBot.Options;
using IsDownBot.Services;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Builder;
using Telegram.Bot.Builder.Extensions;
using Telegram.Bot.Builder.Hosting;
using Telegram.Bot.Builder.UpdateHandling;
using Telegram.Bot.Types.Enums;

namespace IsDownBot
{
    public class Startup : IStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddFilters();
            services.AddControllersServices(config =>
            {
                config.AddFilter<AdminControllerFilter>();
            });
            services.AddOptions<UsersOptions>()
                .Configure(opt =>
                {
                    opt.AdminIds = GetUserIdsFromEnvironment();
                });
            services.AddScoped<IDownDetector, DownDetector>();
            services.AddSingleton<IUserService, UserService>();
        }

        public void Configure(IApplicationBuilder app)
        {
            // Assume that default command is '/isdown'
            app.Use(UpdateType.Message, (ctx, next) =>
            {
                if (ctx.Command == null && ctx.Update.Message.Text.IsValidUri())
                {
                    ctx = new UpdateContext(
                        ctx.BotClient,
                        ctx.Update,
                        new BotCommand("isdown",  botName: null, new [] {new BotCommand.Argument(ctx.Update.Message.Text, null)}));
                }
                return next(ctx);
            });

            app.UseControllers();
        }

        private IReadOnlyCollection<int>? GetUserIdsFromEnvironment()
        {
            var env = Environment.GetEnvironmentVariable("ADMIN_IDS");

            return env?.Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Where(i => int.TryParse(i, out _))
                .Select(int.Parse)
                .ToArray();
        }
    }
}
