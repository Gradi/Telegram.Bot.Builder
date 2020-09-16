using System;
using System.Collections.Generic;
using Telegram.Bot.Builder.Controllers.Descriptors;
using Telegram.Bot.Builder.Controllers.Exceptions;

namespace Telegram.Bot.Builder.Controllers.Binders
{
    internal class ArgumentBinder : IArgumentBinder
    {
        private readonly object _null;

        public ArgumentBinder()
        {
            _null = new object();
        }

        public object[] Bind(ActionDescriptor action, IEnumerable<BotCommand.Argument> arguments)
        {
            var actualArguments = GetInitialArray(action);
            int index = 0;

            foreach (var inputArg in arguments)
            {
                if (index == action.Arguments.Count)
                    throw new ArgumentBindingException("Got too many arguments.");

                ArgumentDescriptor descriptor = null!;
                if (inputArg.IsNamed)
                {
                    if (!action.TryGetArgumentByName(inputArg.Name!, out descriptor))
                        throw new ArgumentBindingException($"Can't get argument by it's name: \"{inputArg.Name}\".");
                }
                else
                {
                    descriptor = action.GetArgumentByIndex(index);
                    index += 1;
                }

                try
                {
                    var bindingResult = descriptor.Binder.Bind(inputArg.Value, descriptor.Parameter.Name, descriptor.Parameter.ParameterType);
                    if (!bindingResult.IsSuccessful) throw new Exception("Binder returned unsuccessful binder result.");
                    actualArguments[descriptor.Parameter.Position] = bindingResult.Value!;
                }
                catch(Exception exception)
                {
                    throw new ArgumentBindingException($"Binder of type \"{descriptor.Binder.GetType()}\" has thrown exception while binding " +
                                                       $"\"{inputArg}\" to \"{descriptor}\".", descriptor, exception);
                }
            }

            TryBindMissingParams(actualArguments, action);
            return actualArguments;
        }

        private void TryBindMissingParams(object[] arguments, ActionDescriptor actionDescriptor)
        {
            List<string>? failedParams = null;
            for (int i = 0; i < arguments.Length; ++i)
            {
                if (IsUnbindedParameter(arguments[i]))
                {
                    var info = actionDescriptor.GetArgumentByIndex(i).Parameter;
                    if (info.HasDefaultValue)
                    {
                        arguments[i] = info.DefaultValue!;
                    }
                    else
                    {
                        failedParams ??= new List<string>();
                        failedParams.Add(info.Name);
                    }
                }
            }

            if (failedParams != null)
            {
                throw new ArgumentBindingException("Can't bind some arguments (they either are missing in input command or doesn't have default values)" +
                                                   $"{Environment.NewLine}{string.Join(", ", failedParams)}");
            }
        }

        private object[] GetInitialArray(ActionDescriptor actionDescriptor)
        {
            var result = new object[actionDescriptor.Arguments.Count];
            for(int i = 0; i < result.Length; ++i)
            {
                result[i] = _null;
            }
            return result;
        }

        private bool IsUnbindedParameter(object input) => ReferenceEquals(input, _null);
    }
}
