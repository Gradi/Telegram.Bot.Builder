using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Telegram.Bot.Builder.Controllers.Attributes;
using Telegram.Bot.Builder.Controllers.Binders;
using Telegram.Bot.Builder.Extensions;

namespace Telegram.Bot.Builder.Controllers.Descriptors
{
    public class ControllerDescriptor
    {
        private readonly IReadOnlyDictionary<string, ActionDescriptor> _actionsByName;

        public string Name { get; }

        public Type Type { get; }

        public IReadOnlyCollection<ActionDescriptor> Actions { get; }

        public ActionDescriptor? DefaultAction { get; }

        public IReadOnlyCollection<Type> Filters { get; }

        internal ControllerDescriptor
            (
                Type controllerType,
                IEnumerable<IBinder> binders
            )
        {
            CheckControllerType(controllerType);

            Name = controllerType.GetCustomAttribute<CommandAttribute>()?.Name ?? controllerType.Name.TrimEnd(nameof(Controller))!;
            Type = controllerType;

            var actions = GetActions(controllerType).Select(m => new ActionDescriptor(this, m, binders)).ToList();
            if (actions.Count(a => a.IsDefault) > 1)
            {
                throw new ArgumentException($"Controller \"{controllerType}\" can only have single or none methods marked with \"{nameof(DefaultCommandAttribute)}\".");
            }
            Actions = actions.Where(a => !a.IsDefault).ToArray();
            DefaultAction = actions.SingleOrDefault(a => a.IsDefault);
            Filters = controllerType.GetCustomAttributes<FilterAttribute>().Select(fattr => fattr.Type).ToArray();

            _actionsByName = Actions.ToDictionary(a => a.Name.ToLowerInvariant(), a => a);
        }

        public bool TryGetActionByName(string name, out ActionDescriptor actionDescriptor) =>
            _actionsByName.TryGetValue(name.ToLowerInvariant(), out actionDescriptor);

        public override string ToString() =>
            $"{Type.Name} {{{string.Join(", ", Actions.Select(a => a.Method.Name))}}}";

        public static IEnumerable<Type> GetControllers(Assembly assembly)
        {
            return assembly
                .GetTypes()
                .Where(t => t.IsValidFinalImplementationOf<Controller>())
                .Where(t => t.IsPublic)
                .Where(t => !t.IsDefined(typeof(IgnoreAttribute)))
                .Where(t => t.Name.EndsWith(nameof(Controller)));
        }

        private static void CheckControllerType(Type controllerType)
        {
            if (controllerType == null) throw new ArgumentNullException(nameof(controllerType));

            var errors = new List<Exception>();

            try
            {
                controllerType.IsValidFinalImplementationOfOrThrow<Controller>();
            }
            catch(Exception exception)
            {
                errors.Add(exception);
            }

            if (controllerType.IsDefined(typeof(IgnoreAttribute)))
            {
                errors.Add(new ArgumentException($"Controller \"{controllerType}\" is marked with \"{typeof(IgnoreAttribute)}\" attribute."));
            }
            if (!controllerType.Name.EndsWith(nameof(Controller)))
            {
                errors.Add(new ArgumentException($"Controller's \"{controllerType}\" name must end with \"{nameof(Controller)}\" word."));
            }

            if (errors.Count != 0)
            {
                throw new AggregateException(errors);
            }
        }

        private static IEnumerable<MethodInfo> GetActions(Type controllerType)
        {
            return controllerType
                .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy)
                .Where(m => !m.IsDefined(typeof(IgnoreAttribute)))
                .Where(m => m.DeclaringType != typeof(object));
        }
    }
}
