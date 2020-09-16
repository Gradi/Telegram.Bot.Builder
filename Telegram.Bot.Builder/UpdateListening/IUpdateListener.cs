using System.Threading.Tasks;

namespace Telegram.Bot.Builder.UpdateListening
{
    public delegate void OnUpdate(object sender, UpdateEventArgs args);

    /// <summary>
    /// Interface for listening for updates.
    /// </summary>
    public interface IUpdateListener
    {
        /// <summary>
        /// An event that is fired on every update arrival.
        /// </summary>
        event OnUpdate? OnUpdate;

        /// <summary>
        /// Starts current listener. Must not block calling thread.
        /// </summary>
        Task StartAsync();

        /// <summary>
        /// Stops current listener. Must not block calling thread.
        /// </summary>
        Task StopAsync();
    }
}
