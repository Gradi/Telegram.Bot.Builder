namespace Telegram.Bot.Builder.Controllers.Binders
{
    public readonly struct BindingResult
    {
        public readonly bool IsSuccessful;
        public readonly object? Value;

        private BindingResult(bool isSuccessful, object? value)
        {
            IsSuccessful = isSuccessful;
            Value = value;
        }

        public static BindingResult Success(object? value) => new BindingResult(true, value);

        public static BindingResult Fail() => new BindingResult(false, null);
    }
}
