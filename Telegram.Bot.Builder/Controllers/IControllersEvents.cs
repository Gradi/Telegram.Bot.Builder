using System;
using System.Threading.Tasks;
using Telegram.Bot.Builder.Controllers.Descriptors;
using Telegram.Bot.Builder.Controllers.Filters;
using Telegram.Bot.Builder.Extensions;
using Telegram.Bot.Builder.UpdateHandling;

namespace Telegram.Bot.Builder.Controllers
{
    /// <summary>
    /// Interface that accepts events during handling update by controllers during different stages.
    /// Default implementation does nothing.
    /// Add your implementation before calling 'AddControllersServices()'
    /// </summary>
    public interface IControllersEvents
    {
        Task<bool> OnUpdateArrival(UpdateContext context);

        Task OnControllerNotFound(UpdateContext context);
        Task<bool> OnControllerFound(UpdateContext context, ControllerDescriptor controllerDescriptor);

        Task OnActionNotFound(UpdateContext context, ControllerDescriptor controllerDescriptor);
        Task<bool> OnActionFound(UpdateContext context, ActionDescriptor actionDescriptor);

        Task OnArgumentBindingFailure(UpdateContext context, ActionDescriptor actionDescriptor,  ArgumentDescriptor? failedArg, Exception exception);

        Task OnPrefiltering(UpdateContext context, ControllerContext controllerContext);
        Task OnPrefilteringCancellation(UpdateContext context, ControllerContext controllerContext, IFilter filter);

        Task OnPreActionInvocation(UpdateContext context, ControllerContext controllerContext);
        Task OnActionThrownException(UpdateContext context, ControllerContext controllerContext);

        Task OnPostFiltering(UpdateContext context, ControllerContext controllerContext);
        Task OnPostFilteringCancellation(UpdateContext context, ControllerContext controllerContext, IFilter filter);

        Task OnControllerHandlingComplete(UpdateContext context);

        Task OnGeneralError(UpdateContext context, Exception exception);
    }
}
