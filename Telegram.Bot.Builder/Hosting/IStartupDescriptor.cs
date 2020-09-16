using System;
using Microsoft.Extensions.DependencyInjection;

namespace Telegram.Bot.Builder.Hosting
{
    internal interface IStartupDescriptor
    {
        Type Type { get; }

        IStartup Instance { get; }

        void ConfigureServices(IServiceCollection services);

        void Configure(IApplicationBuilder builder, IServiceProvider serviceProvider);
    }
}
