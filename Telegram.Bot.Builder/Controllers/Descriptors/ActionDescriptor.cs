using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Builder.Controllers.Attributes;
using Telegram.Bot.Builder.Controllers.Binders;

namespace Telegram.Bot.Builder.Controllers.Descriptors
{
    /// <summary>
    /// Describes single action from controller.
    /// </summary>
    public class ActionDescriptor
    {
        private readonly Func<Controller, object[], Task> _action;
        private readonly ArgumentDescriptor[] _arguments;
        private readonly IReadOnlyDictionary<string, ArgumentDescriptor> _argumentsByName;

        /// <summary>
        /// Name of action. It is either taken from <see cref="CommandAttribute"/> or
        /// name of method.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Flag if it is default action.
        /// </summary>
        public bool IsDefault { get; }

        /// <summary>
        /// Method definition.
        /// </summary>
        public MethodInfo Method { get; }

        /// <summary>
        /// Collection of method arguments.
        /// </summary>
        public IReadOnlyCollection<ArgumentDescriptor> Arguments => _arguments;

        /// <summary>
        /// Collection of filters this action is marked with.
        /// </summary>
        public IReadOnlyCollection<Type> Filters { get; }

        /// <summary>
        /// Descriptor of a controller this action belongs to.
        /// </summary>
        public ControllerDescriptor Controller { get; }

        internal ActionDescriptor
            (
                ControllerDescriptor controllerDescriptor,
                MethodInfo method,
                IEnumerable<IBinder> binders
            )
        {
            if (controllerDescriptor == null) throw new ArgumentNullException(nameof(controllerDescriptor));
            if (method == null) throw new ArgumentNullException(nameof(method));

            Name = method.GetCustomAttribute<CommandAttribute>()?.Name ?? method.Name;
            IsDefault = method.IsDefined(typeof(DefaultCommandAttribute));
            Method = method;
            Filters = method.GetCustomAttributes<FilterAttribute>().Select(fattr => fattr.Type).ToArray();
            Controller = controllerDescriptor;

            _action = CreateAction(method);
            _arguments = method.GetParameters().Select(p => new ArgumentDescriptor(this, p, binders)).ToArray();
            _argumentsByName = _arguments.ToDictionary(a => a.Parameter.Name, a => a);
        }

        /// <summary>
        /// Invokes this action.
        /// </summary>
        /// <param name="instance">Instance of a controller.</param>
        /// <param name="arguments">Method arguments.</param>
        public Task Invoke(Controller instance, object[] arguments) => _action(instance, arguments);

        /// <summary>
        /// Returns argument by it's index.
        /// </summary>
        public ArgumentDescriptor GetArgumentByIndex(int index) => _arguments[index];

        /// <summary>
        /// Attempts to return argument by it's name.
        /// </summary>
        public bool TryGetArgumentByName(string name, out ArgumentDescriptor argumentDescriptor) =>
            _argumentsByName.TryGetValue(name, out argumentDescriptor);

        public override string ToString()
        {
            return new StringBuilder()
                .Append(Controller.Type.Name).Append('.').Append(Method.Name)
                .Append('(')
                .Append(string.Join(", ", Arguments.Select(a => $"{a.Parameter.ParameterType.Name} {a.Parameter.Name}")))
                .Append(')')
                .ToString();
        }

        private static Func<Controller, object[], Task> CreateAction(MethodInfo method)
        {
            var controller = Expression.Parameter(typeof(Controller), "controller");
            var arguments = Expression.Parameter(typeof(object[]), "args");

            var actualArgs = method.GetParameters().Select(para =>
                Expression.Convert(Expression.ArrayAccess(arguments, Expression.Constant(para.Position)), para.ParameterType));

            Expression body = Expression.Convert(controller, method.DeclaringType!);
            body = Expression.Call(body, method, actualArgs);

            if (method.ReturnType == typeof(void))
            {
                var completedTask = typeof(Task).GetProperty(nameof(Task.CompletedTask), BindingFlags.Public | BindingFlags.Static)!;
                var completedTaskExpr = Expression.MakeMemberAccess(null, completedTask);

                body = Expression.Block(typeof(Task), body, completedTaskExpr);
            }
            else if (method.ReturnType != typeof(Task))
            {
                throw new ArgumentException($"Method \"{method}\" must return either void or Task, but it returns \"{method.ReturnType}\".");
            }

            var lambda = Expression.Lambda<Func<Controller, object[], Task>>(body, controller, arguments);
            return lambda.Compile();
        }
    }
}
