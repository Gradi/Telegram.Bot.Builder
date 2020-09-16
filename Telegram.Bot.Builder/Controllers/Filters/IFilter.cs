using System.Threading.Tasks;

namespace Telegram.Bot.Builder.Controllers.Filters
{
    /// <summary>
    /// Interface of a filter.
    /// Filter is invoked just before and after invocation of controller's action.
    /// Filter can cancel invocation of action and following filters.
    /// </summary>
    public interface IFilter
    {
        /// <summary>
        /// This method gets call before invocation of controller's action.
        /// </summary>
        Task OnActionInvocation(ControllerContext context);

        /// <summary>
        /// This method gets call after invocation of controller's action.
        /// </summary>
        Task PostActionInvocation(ControllerContext context);
    }
}
