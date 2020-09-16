using System;
using System.Threading.Tasks;
using Telegram.Bot.Builder.Controllers.Descriptors;
using Telegram.Bot.Builder.Controllers.Filters;
using Telegram.Bot.Builder.UpdateHandling;

namespace Telegram.Bot.Builder.Controllers
{
    internal class NullContollerEvents : IControllersEvents
    {
        private readonly Task<bool> _trueTask;
        private readonly Task _task;

        public NullContollerEvents()
        {
            _trueTask = Task.FromResult(true);
            _task = Task.CompletedTask;
        }

        public Task<bool> OnUpdateArrival(UpdateContext context) => _trueTask;

        public Task OnControllerNotFound(UpdateContext context) => _task;

        public Task<bool> OnControllerFound(UpdateContext context, ControllerDescriptor controllerDescriptor) =>
            _trueTask;

        public Task OnActionNotFound(UpdateContext context, ControllerDescriptor controllerDescriptor) => _task;

        public Task<bool> OnActionFound(UpdateContext context, ActionDescriptor actionDescriptor) => _trueTask;

        public Task OnArgumentBindingFailure(UpdateContext context, ActionDescriptor actionDescriptor, ArgumentDescriptor? failedArg, Exception exception) =>
            _task;

        public Task OnPrefiltering(UpdateContext context, ControllerContext controllerContext) => _task;

        public Task OnPrefilteringCancellation(UpdateContext context, ControllerContext controllerContext, IFilter filter) =>
            _task;

        public Task OnPreActionInvocation(UpdateContext context, ControllerContext controllerContext) => _task;

        public Task OnActionThrownException(UpdateContext context, ControllerContext controllerContext) => _task;

        public Task OnPostFiltering(UpdateContext context, ControllerContext controllerContext) => _task;

        public Task OnPostFilteringCancellation(UpdateContext context, ControllerContext controllerContext, IFilter filter) =>
            _task;

        public Task OnControllerHandlingComplete(UpdateContext context) => _task;

        public Task OnGeneralError(UpdateContext context, Exception exception) => _task;
    }
}
