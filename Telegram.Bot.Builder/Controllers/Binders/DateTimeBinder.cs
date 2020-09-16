using System;

namespace Telegram.Bot.Builder.Controllers.Binders
{
    internal class DateTimeBinder : BaseBinder<DateTime>
    {
        public override BindingResult Bind(string? value, string name, Type type)
        {
            if (DateTime.TryParse(value, out var result))
                return BindingResult.Success(result);

            return BindingResult.Fail();
        }
    }
}
