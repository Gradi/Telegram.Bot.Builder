using System.Collections.Generic;

namespace Telegram.Bot.Builder.Controllers.Descriptors
{
    public interface IControllerDescriptorCollection : IReadOnlyCollection<ControllerDescriptor>
    {
        bool TryGetControllerByName(string name, out ControllerDescriptor controllerDescriptor);
    }
}
