using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Telegram.Bot.Builder.Controllers.Attributes;
using Telegram.Bot.Builder.Controllers.Binders;

namespace Telegram.Bot.Builder.Controllers.Descriptors
{
    /// <summary>
    /// Describes single argument from controller's action.
    /// </summary>
    public class ArgumentDescriptor
    {
        /// <summary>
        /// Parameter info.
        /// </summary>
        public ParameterInfo Parameter { get; }

        /// <summary>
        /// Implementation of <see cref="IBinder"/> that is capable of binding this argument.
        /// </summary>
        public IBinder Binder { get; }

        /// <summary>
        /// Action this argument belongs to.
        /// </summary>
        public ActionDescriptor Action { get; }

         internal ArgumentDescriptor
            (
                ActionDescriptor action,
                ParameterInfo parameterInfo,
                IEnumerable<IBinder> binders
            )
         {
             if (action == null) throw new ArgumentNullException(nameof(action));
             if (parameterInfo == null) throw new ArgumentNullException(nameof(parameterInfo));

             Parameter = parameterInfo;

             Binder = parameterInfo
                 .GetCustomAttributes<BinderAttribute>()
                 .Concat(parameterInfo.ParameterType.GetCustomAttributes<BinderAttribute>())
                 .Select(battr =>
                 {
                     try
                     {
                         return (IBinder) Activator.CreateInstance(battr.Type);
                     }
                     catch (Exception exception)
                     {
                         throw new Exception($"Can't create instance of \"{battr.Type}\" binder.", exception);
                     }
                 })
                 .FirstOrDefault() ??
                 binders.FirstOrDefault(b => b.IsTypeSupported(parameterInfo.ParameterType)) ??
                 throw new Exception($"Can't find binder for argument of type \"{parameterInfo.ParameterType}\".");

             Action = action;
         }

         public override string ToString()
         {
             var builder = new StringBuilder();
             builder.Append(Action.Controller.Type.Name).Append('.').Append(Action.Method.Name).Append('(');
             if (Parameter.Position != 0)
                builder.Append("..., ");

             builder.Append(Parameter.ParameterType.Name).Append(' ').Append(Parameter.Name);

             if (Parameter.Position != Action.Arguments.Count - 1)
                builder.Append(", ...");

             builder.Append(')');

             return builder.ToString();
         }
    }
}
