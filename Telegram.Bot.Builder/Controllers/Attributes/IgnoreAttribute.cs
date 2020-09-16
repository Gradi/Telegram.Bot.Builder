using System;

namespace Telegram.Bot.Builder.Controllers.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class IgnoreAttribute : Attribute {}
}
