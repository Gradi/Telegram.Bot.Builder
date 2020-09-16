using System;

namespace Telegram.Bot.Builder.Controllers.Binders
{
    internal class StringBinder : BaseBinder<string>
    {
        public override BindingResult Bind(string? value, string name, Type type) =>
            value != null ? BindingResult.Success(value) : BindingResult.Fail();
    }
}
