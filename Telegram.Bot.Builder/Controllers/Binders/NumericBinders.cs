using System;

namespace Telegram.Bot.Builder.Controllers.Binders
{
    internal abstract class NumberBinder<T> : BaseBinder<T> where T : struct
    {
        public override BindingResult Bind(string? value, string name, Type type)
        {
            if (TryParse(value, out T result))
                return BindingResult.Success(result);

            return BindingResult.Fail();
        }

        protected abstract bool TryParse(string? value, out T result);
    }

    internal class ByteBinder : NumberBinder<byte>
    {
        protected override bool TryParse(string? value, out byte result) =>
            byte.TryParse(value, out result);
    }

    internal class SByteBinder : NumberBinder<sbyte>
    {
        protected override bool TryParse(string? value, out sbyte result) =>
            sbyte.TryParse(value, out result);
    }

    internal class ShortBinder : NumberBinder<short>
    {
        protected override bool TryParse(string? value, out short result) =>
            short.TryParse(value, out result);
    }

    internal class UshortBinder : NumberBinder<ushort>
    {
        protected override bool TryParse(string? value, out ushort result) =>
            ushort.TryParse(value, out result);
    }

    internal class IntBinder : NumberBinder<int>
    {
        protected override bool TryParse(string? value, out int result) =>
            int.TryParse(value, out result);
    }

    internal class UintBinder : NumberBinder<uint>
    {
        protected override bool TryParse(string? value, out uint result) =>
            uint.TryParse(value, out result);
    }

    internal class LongBinder : NumberBinder<long>
    {
        protected override bool TryParse(string? value, out long result) =>
            long.TryParse(value, out result);
    }

    internal class UlongBinder : NumberBinder<ulong>
    {
        protected override bool TryParse(string? value, out ulong result) =>
            ulong.TryParse(value, out result);
    }

    internal class FloatBinder : NumberBinder<float>
    {
        protected override bool TryParse(string? value, out float result) =>
            float.TryParse(value, out result);
    }

    internal class DoubleBinder : NumberBinder<double>
    {
        protected override bool TryParse(string? value, out double result) =>
            double.TryParse(value, out result);
    }

    internal class DecimalBinder : NumberBinder<decimal>
    {
        protected override bool TryParse(string? value, out decimal result) =>
            decimal.TryParse(value, out result);
    }
}
