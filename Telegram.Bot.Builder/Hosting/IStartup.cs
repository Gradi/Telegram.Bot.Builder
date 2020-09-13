using Microsoft.Extensions.DependencyInjection;

namespace Telegram.Bot.Builder.Hosting
{
    public interface IStartup
    {
        void ConfigureServices(IServiceCollection services);
    }
}
