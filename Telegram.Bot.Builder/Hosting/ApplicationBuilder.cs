using System;
using System.Collections.Generic;
using System.Linq;
using Telegram.Bot.Builder.UpdateHandling;

namespace Telegram.Bot.Builder.Hosting
{
    internal class ApplicationBuilder : IApplicationBuilder
    {
        private readonly ICollection<IAdvancedUpdateHandler> _handlers;

        public ApplicationBuilder()
        {
            _handlers = new List<IAdvancedUpdateHandler>();
        }

        public IApplicationBuilder Use<T>() where T : class, IUpdateHandler
        {
            _handlers.Add(new DiUpdateHandler(typeof(T)));
            return this;
        }

        public IApplicationBuilder Use(IUpdateHandler handler)
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));

            _handlers.Add(new FromInstanceUpdateHandler(handler));
            return this;
        }

        public IAdvancedUpdateHandler Build()
        {
            if (_handlers.Count == 0)
            {
                return new EmptyUpdateHandler();
            }
            if (_handlers.Count == 1)
            {
                return _handlers.Single();
            }

            ChainedUpdateHandler? head = null;
            ChainedUpdateHandler? tail = null;
            foreach (var handler in _handlers)
            {
                if (head == null)
                {
                    head = ChainedUpdateHandler.New(handler);
                    tail = head;
                }
                else
                {
                    tail = tail!.Add(handler);
                }
            }
            return head!;
        }
    }
}
