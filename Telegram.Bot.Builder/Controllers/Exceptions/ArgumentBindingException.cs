using System;
using Telegram.Bot.Builder.Controllers.Descriptors;

namespace Telegram.Bot.Builder.Controllers.Exceptions
{
    internal class ArgumentBindingException : Exception
    {
        public ArgumentDescriptor? FailedArgument { get; }

        public ArgumentBindingException(string message, ArgumentDescriptor? failedArgument = null, Exception? innerException = null)
            : base(message, innerException)
        {
            FailedArgument = failedArgument;
        }
    }
}
