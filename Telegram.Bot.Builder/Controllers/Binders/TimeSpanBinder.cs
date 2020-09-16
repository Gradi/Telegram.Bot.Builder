using System;

namespace Telegram.Bot.Builder.Controllers.Binders
{
    public class TimeSpanBinder : BaseBinder<TimeSpan>
    {
        public override BindingResult Bind(string? value, string name, Type type)
        {
            if (TimeSpan.TryParse(value, out var result))
                return BindingResult.Success(result);

            return BindingResult.Fail();
        }
    }
}
