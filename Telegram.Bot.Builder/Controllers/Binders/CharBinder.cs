using System;

namespace Telegram.Bot.Builder.Controllers.Binders
{
    internal class CharBinder : BaseBinder<char>
    {
        public override BindingResult Bind(string? value, string name, Type type)
        {
            if (value == null || value.Length == 0 || value.Length > 1)
                return BindingResult.Fail();

            return BindingResult.Success(value[0]);
        }
    }
}
