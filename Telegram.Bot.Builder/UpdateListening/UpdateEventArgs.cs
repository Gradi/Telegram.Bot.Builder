using System;
using Telegram.Bot.Types;

namespace Telegram.Bot.Builder.UpdateListening
{
    public class UpdateEventArgs : EventArgs
    {
        public Update Update { get; }

        public UpdateEventArgs(Update update)
        {
            Update = update;
        }
    }
}
