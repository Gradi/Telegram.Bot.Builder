using System;
using System.Collections.Generic;

namespace Telegram.Bot.Builder.Controllers.Binders
{
    internal class BoolBinder : BaseBinder<bool>
    {
        private readonly HashSet<string> _trueStrings;
        private readonly HashSet<string> _falseStrings;
        private readonly BindingResult _trueResult;
        private readonly BindingResult _falseResult;

        public BoolBinder()
        {
            _trueStrings = new HashSet<string>
            {
                "true", "yes", "y", "da"
            };
            _falseStrings = new HashSet<string>
            {
                "false", "no", "n", "net"
            };
            _trueResult = BindingResult.Success(true);
            _falseResult = BindingResult.Success(false);
        }

        public override BindingResult Bind(string? value, string name, Type type)
        {
            if (string.IsNullOrWhiteSpace(value))
                return BindingResult.Fail();

            value = value.ToLowerInvariant();

            if (_trueStrings.Contains(value))
                return _trueResult;

            if (_falseStrings.Contains(value))
                return _falseResult;

            return BindingResult.Fail();
        }
    }
}
