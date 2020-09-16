using System;

namespace Telegram.Bot.Builder.Controllers.Binders
{
    internal class DateTimeOffsetBinder : BaseBinder<DateTimeOffset>
    {
        public override BindingResult Bind(string? value, string name, Type type)
        {
            if (DateTimeOffset.TryParse(value, out var result))
                return BindingResult.Success(result);

            return BindingResult.Fail();
        }
    }
}
