using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Options;

namespace Telegram.Bot.Builder.Controllers.Descriptors
{
    internal class ControllerDescriptorCollection : IControllerDescriptorCollection
    {
        private readonly IReadOnlyCollection<ControllerDescriptor> _controllers;
        private readonly IReadOnlyDictionary<string, ControllerDescriptor> _controllersByName;

        public int Count => _controllers.Count;

        public ControllerDescriptorCollection(IOptions<ControllerOptions> controllerOptions)
        {
            _controllers = controllerOptions.Value.ControllerAssemblies
                .Select(ControllerDescriptor.GetControllers)
                .SelectMany(a => a)
                .Select(t => new ControllerDescriptor(t, controllerOptions.Value.Binders))
                .ToArray();

            _controllersByName = _controllers.ToDictionary(c => c.Name.ToLowerInvariant(), c => c);
        }

        public bool TryGetControllerByName(string name, out ControllerDescriptor controllerDescriptor) =>
            _controllersByName.TryGetValue(name.ToLowerInvariant(), out controllerDescriptor);

        public IEnumerator<ControllerDescriptor> GetEnumerator() => _controllers.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
