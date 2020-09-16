using System.Threading.Tasks;

namespace Telegram.Bot.Builder.Controllers.Filters
{
    public interface IFilter
    {
        Task OnActionInvocation(ControllerContext context);

        Task PostActionInvocation(ControllerContext context);
    }
}
