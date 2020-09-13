using System.Threading.Tasks;

namespace Telegram.Bot.Builder.UpdateListening
{
    public delegate void OnUpdate(object sender, UpdateEventArgs args);

    public interface IUpdateListener
    {
        event OnUpdate? OnUpdate;

        Task StartAsync();

        Task StopAsync();
    }
}
