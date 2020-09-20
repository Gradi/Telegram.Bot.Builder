using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot.Builder.Controllers.Binders;
using Telegram.Bot.Builder.Controllers.Descriptors;
using Telegram.Bot.Builder.Controllers.Exceptions;
using Telegram.Bot.Builder.Controllers.Filters;
using Telegram.Bot.Builder.UpdateHandling;

namespace Telegram.Bot.Builder.Controllers
{
    internal class ControllersUpdateHandler : IUpdateHandler
    {
        private readonly ILogger _logger;
        private readonly IServiceProvider _services;
        private readonly IControllerDescriptorCollection _controllers;
        private readonly IControllersEvents _controllersEvents;
        private readonly IArgumentBinder _argumentBinder;
        private readonly IEnumerable<Type> _globalFilters;

        public ControllersUpdateHandler
            (
                ILogger<ControllersUpdateHandler> logger,
                IServiceProvider serviceProvider,
                IControllerDescriptorCollection controllers,
                IControllersEvents controllersEvents,
                IArgumentBinder argumentBinder,
                IOptionsSnapshot<ControllerOptions> options
            )
        {
            _services = serviceProvider;
            _controllers = controllers;
            _logger = logger;
            _controllersEvents = controllersEvents;
            _argumentBinder = argumentBinder;
            _globalFilters = options.Value.Filters;
        }

        public async Task HandleAsync(UpdateContext context, Func<UpdateContext, Task> next)
        {
            if (!(await _controllersEvents.OnUpdateArrival(context)))
            {
                LogEventCancelled(_controllersEvents, nameof(IControllersEvents.OnUpdateArrival));
                return;
            }

            bool isHandled = false;
            try
            {
                isHandled = await HandleController(context);
            }
            catch(Exception exception)
            {
                await _controllersEvents.OnGeneralError(context, exception);
                throw;
            }

            if (isHandled)
            {
                await _controllersEvents.OnControllerHandlingComplete(context);
            }
            else
            {
                await next(context);
            }
        }

        private async Task<bool> HandleController(UpdateContext context)
        {
            if (context.Command == null)
            {
                _logger.LogTrace("Update context is missing bot command.");
                return false;
            }

            if (_controllers.TryGetControllerByName(context.Command.Command, out var controllerDescriptor))
            {
                _logger.LogTrace("Found \"{Controller}\" controller for \"{Command}\" command.", controllerDescriptor, context.Command);
                if (!(await _controllersEvents.OnControllerFound(context, controllerDescriptor)))
                {
                    LogEventCancelled(_controllersEvents, nameof(IControllersEvents.OnControllerFound));
                    return true;
                }

                return await HandleAction(context, controllerDescriptor);
            }
            else
            {
                _logger.LogTrace("Can't find controller to handle \"{Command}\" command.", context.Command.Command);
                await _controllersEvents.OnControllerNotFound(context);
                return false;
            }
        }

        private async Task<bool> HandleAction(UpdateContext context, ControllerDescriptor controllerDescriptor)
        {
            if (!TryGetAction(context, controllerDescriptor, out var result))
            {
                _logger.LogTrace("Can't find action in \"{Controller}\" controller for \"{Command}\" command.",
                    controllerDescriptor, context.Command);
                await _controllersEvents.OnActionNotFound(context, controllerDescriptor);
                return false;
            }
            _logger.LogTrace("Found \"{Action}\" action for \"{Command}\" command.", result.Descriptor, context.Command);
            if (!(await _controllersEvents.OnActionFound(context, result.Descriptor)))
            {
                LogEventCancelled(_controllersEvents, nameof(IControllersEvents.OnActionFound));
                return true;
            }

            var arguments = await TryBindArguments(context, result.Descriptor, result.Args);
            if (arguments == null)
                return true;

            return await InvokeAction(context, result.Descriptor, arguments);
        }

        private async Task<bool> InvokeAction(UpdateContext context, ActionDescriptor action, object[] arguments)
        {
            var controllerContext = new ControllerContext(action, arguments, (Controller)_services.GetRequiredService(action.Controller.Type),
                                                          context);
            controllerContext.Controller.Setup(context);
            var filters = GetFilters(action);

            await _controllersEvents.OnPrefiltering(context, controllerContext);
            if (controllerContext.IsCanceled)
            {
                LogEventCancelled(_controllersEvents, nameof(IControllersEvents.OnPrefiltering));
                return true;
            }

            foreach (var filter in filters)
            {
                await filter.OnActionInvocation(controllerContext);
                if (controllerContext.IsCanceled)
                {
                    LogEventCancelled(filter, nameof(IFilter.OnActionInvocation));
                    await _controllersEvents.OnPrefilteringCancellation(context, controllerContext, filter);
                    return true;
                }
            }

            await _controllersEvents.OnPreActionInvocation(context, controllerContext);
            if (controllerContext.IsCanceled)
            {
                LogEventCancelled(_controllersEvents, nameof(IControllersEvents.OnPreActionInvocation));
                return true;
            }

            try
            {
                await action.Invoke(controllerContext.Controller, arguments);
            }
            catch(Exception exception)
            {
                _logger.LogError(exception, "Action \"{Action}\" has thrown an exception.", action);
                controllerContext.Exception = exception;
                await _controllersEvents.OnActionThrownException(context, controllerContext);
            }

            await _controllersEvents.OnPostFiltering(context, controllerContext);
            if (controllerContext.IsCanceled)
            {
                LogEventCancelled(_controllersEvents, nameof(IControllersEvents.OnPostFiltering));
                return true;
            }

            foreach (var filter in filters)
            {
                await filter.PostActionInvocation(controllerContext);
                if (controllerContext.IsCanceled)
                {
                    LogEventCancelled(filter, nameof(IFilter.PostActionInvocation));
                    await _controllersEvents.OnPostFilteringCancellation(context, controllerContext, filter);
                    return true;
                }
            }

            return true;
        }

        private bool TryGetAction(UpdateContext context, ControllerDescriptor controller, out (ActionDescriptor Descriptor, IEnumerable<BotCommand.Argument> Args) result)
        {
            var command = context.Command!;

            if (command.Arguments.Count != 0 &&
                !command.Arguments.First().IsNamed &&
                controller.TryGetActionByName(command.Arguments.First().Value, out var action))
            {
                result = (action, command.Arguments.Skip(1));
                return true;
            }
            else if (controller.DefaultAction != null)
            {
                result = (controller.DefaultAction, command.Arguments);
                return true;
            }

            result = default;
            return false;
        }

        private async Task<object[]?> TryBindArguments(UpdateContext context, ActionDescriptor action, IEnumerable<BotCommand.Argument> arguments)
        {
            try
            {
                return _argumentBinder.Bind(action, arguments);
            }
            catch(ArgumentBindingException exception)
            {
                _logger.LogError(exception, "Error on binding \"{Arguments}\" arguments for \"{Action}\" action",
                    arguments, action);
                await _controllersEvents.OnArgumentBindingFailure(context, action, exception.FailedArgument, exception);
                return null;
            }
        }

        private IEnumerable<IFilter> GetFilters(ActionDescriptor action)
        {
            var result = new List<IFilter>();

            foreach (var filter in action.Filters)
                result.Add((IFilter)_services.GetRequiredService(filter));

            foreach (var filter in action.Controller.Filters)
                result.Add((IFilter)_services.GetRequiredService(filter));

            foreach (var filter in _globalFilters)
                result.Add((IFilter)_services.GetRequiredService(filter));

            return result;
        }

        private void LogEventCancelled(object instance, string methodName)
        {
            if (_logger.IsEnabled(LogLevel.Trace))
            {
                _logger.LogTrace("\"{TypeName}.{MethodName}\" cancelled controllers update handling.", instance.GetType(), methodName);
            }
        }
    }
}
