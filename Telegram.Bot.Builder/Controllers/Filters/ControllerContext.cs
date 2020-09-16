using System;
using Telegram.Bot.Builder.Controllers.Descriptors;

namespace Telegram.Bot.Builder.Controllers.Filters
{
    public class ControllerContext
    {
        public ActionDescriptor Action { get; }

        public object[] Arguments { get; }

        public ControllerDescriptor ControllerDescriptor => Action.Controller;

        public Controller Controller { get; }

        public bool IsCanceled { get; set; }

        public Exception? Exception { get; set; }

        public ControllerContext
            (
                ActionDescriptor actionDescriptor,
                object[] arguments,
                Controller controller
            )
        {
            Action = actionDescriptor;
            Arguments = arguments;
            Controller = controller;
            IsCanceled = false;
            Exception = null;
        }
    }
}
