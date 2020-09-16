using System;

namespace Telegram.Bot.Builder.Controllers.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited =  false)]
    public class DefaultCommandAttribute : Attribute {}
}
