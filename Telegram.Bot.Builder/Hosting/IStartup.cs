using Microsoft.Extensions.DependencyInjection;

namespace Telegram.Bot.Builder.Hosting
{
    /// <summary>
    /// Startup class for configuring services and update handling chain.
    /// </summary>
    /// <remarks>
    ///     Your implementation must contain 'public void Configure' method that accepts
    /// <see cref="IApplicationBuilder"/> as one of it's arguments (it can also accept any other services
    /// from DI).
    /// </remarks>
    public interface IStartup
    {
        /// <summary>
        /// Configures services for bot application.
        /// </summary>
        void ConfigureServices(IServiceCollection services);
    }
}
