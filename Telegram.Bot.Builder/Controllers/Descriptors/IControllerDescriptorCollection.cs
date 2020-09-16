using System.Collections.Generic;

namespace Telegram.Bot.Builder.Controllers.Descriptors
{
    /// <summary>
    /// Collection of detected controllers.
    /// You can retreieve actual implementation through dependency injection.
    /// </summary>
    public interface IControllerDescriptorCollection : IReadOnlyCollection<ControllerDescriptor>
    {
        /// <summary>
        /// Attempts to get controller descriptor by it's name.
        /// </summary>
        bool TryGetControllerByName(string name, out ControllerDescriptor controllerDescriptor);
    }
}
