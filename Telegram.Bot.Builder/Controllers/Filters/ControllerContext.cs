using System;
using Telegram.Bot.Builder.Controllers.Descriptors;
using Telegram.Bot.Builder.UpdateHandling;

namespace Telegram.Bot.Builder.Controllers.Filters
{
    /// <summary>
    /// Context for controller and action that will be invoked.
    /// </summary>
    public class ControllerContext
    {
        /// <summary>
        /// Descriptor for action being invoked.
        /// </summary>
        public ActionDescriptor Action { get; }

        /// <summary>
        /// Actual arguments for method.
        /// </summary>
        public object[] Arguments { get; }

        /// <summary>
        /// Descriptor of a controller.
        /// </summary>
        public ControllerDescriptor ControllerDescriptor => Action.Controller;

        /// <summary>
        /// Controller instance.
        /// </summary>
        public Controller Controller { get; }

        /// <summary>
        /// Update context
        /// </summary>
        public UpdateContext UpdateContext { get; }

        /// <summary>
        /// Set this flag to cancel action invocation.
        /// </summary>
        public bool IsCanceled { get; set; }

        /// <summary>
        /// If action throws an exception this property will hold it.
        /// </summary>
        public Exception? Exception { get; set; }

        public ControllerContext
            (
                ActionDescriptor actionDescriptor,
                object[] arguments,
                Controller controller,
                UpdateContext updateContext
            )
        {
            Action = actionDescriptor;
            Arguments = arguments;
            Controller = controller;
            UpdateContext = updateContext;
            IsCanceled = false;
            Exception = null;
        }
    }
}
