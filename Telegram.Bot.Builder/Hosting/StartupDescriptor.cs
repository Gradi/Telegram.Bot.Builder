using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Builder.Extensions;

namespace Telegram.Bot.Builder.Hosting
{
    internal class StartupDescriptor : IStartupDescriptor
    {
        private readonly Lazy<IStartup> _startupValue;

        public Type Type => _startupValue.Value.GetType();

        public IStartup Instance => _startupValue.Value;

        public StartupDescriptor(Func<IStartup> startupFactory)
        {
            _startupValue = new Lazy<IStartup>(startupFactory);
        }

        public void ConfigureServices(IServiceCollection services) => Instance.ConfigureServices(services);

        public void Configure(IApplicationBuilder builder, IServiceProvider serviceProvider)
        {
            var configure = Type.GetMethod(nameof(Configure), BindingFlags.Instance | BindingFlags.Public);
            if (!IsValidConfigureMethod(configure))
            {
                throw new Exception($"Your \"{Type}\" startup class must contain a single public method named \"{nameof(Configure)}\" " +
                                    $"that accepts \"{typeof(IApplicationBuilder)}\" as it's any argument (it may also accept any other arguments " +
                                    $"registered through DI) and returns void.");
            }

            var args = configure!
                .GetParameters()
                .Select(p => p.ParameterType == typeof(IApplicationBuilder) ? (object?)builder : null)
                .ToArray();

            configure.InvokeWithServices(Instance, args, serviceProvider);
        }

        private static bool IsValidConfigureMethod(MethodInfo? configureMethod)
        {
            if (configureMethod == null)
                return false;

            var @params = configureMethod.GetParameters();
            return @params.Length != 0 &&
                   @params.Any(p => p.ParameterType == typeof(IApplicationBuilder)) &&
                   configureMethod.ReturnType == typeof(void);
        }
    }
}
