using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Telegram.Bot.Builder.Controllers;
using Telegram.Bot.Builder.Controllers.Binders;
using Telegram.Bot.Builder.Controllers.Descriptors;
using Telegram.Bot.Builder.Controllers.Filters;

namespace Telegram.Bot.Builder.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds controller types to service collection.
        /// </summary>
        /// <param name="services">Service collection.</param>
        /// <param name="lifetime">Controllers lifetime.</param>
        /// <param name="assemblies">Optional. Collection of assemblies to scan for controllers. If null or empty then entry assembly is searched for controllers types.</param>
        /// <exception cref="Exception">If <paramref name="assemblies"/> collection is null or empty and can't get entry assembly.</exception>
        public static IServiceCollection AddControllers(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Transient, params Assembly[]? assemblies)
        {
            if (assemblies == null || assemblies.Length == 0)
            {
                assemblies = new [] { GetEntryAssembly() };
            }

            var types = assemblies
                .Select(ControllerDescriptor.GetControllers)
                .SelectMany(a => a)
                .ToList();

            foreach (var controllerType in types)
            {
                services.Add(new ServiceDescriptor(controllerType, controllerType, lifetime));
            }
            return services;
        }

        /// <summary>
        /// Adds filters to service collection.
        /// </summary>
        /// <param name="services">Service collection.</param>
        /// <param name="lifetime">Filters lfietime.</param>
        /// <param name="assemblies">Optional. Collection of assemblies to scan for controllers Is null or empty then entry assembly is searched for filters types.</param>
        /// <exception cref="Exception">If <paramref name="assemblies"/> collection is null or emptry and can't get entry assembly.</exception>
        public static IServiceCollection AddFilters(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Scoped, params Assembly[]? assemblies)
        {
            if (assemblies == null || assemblies.Length == 0)
            {
                assemblies = new [] { GetEntryAssembly() };
            }

            var types = assemblies
                .Select(a => a.GetTypes())
                .SelectMany(a => a)
                .Where(t => t.IsValidFinalImplementationOf<IFilter>())
                .ToList();

            foreach (var filterType in types)
            {
                services.Add(new ServiceDescriptor(filterType, filterType, lifetime));
            }
            return services;
        }

        /// <summary>
        /// Adds services required for controllers.
        /// </summary>
        public static IServiceCollection AddContollersServices(this IServiceCollection services) =>
            services.AddContollersServices(_ => {});

        /// <summary>
        /// Adds services required for controllers.
        /// </summary>
        public static IServiceCollection AddContollersServices(this IServiceCollection services, Action<ControllerOptions> configureDelegate)
        {
            services.Configure<ControllerOptions>(configureDelegate);

            services.AddSingleton<IControllerDescriptorCollection, ControllerDescriptorCollection>();
            services.AddSingleton<IArgumentBinder, ArgumentBinder>();
            services.TryAddSingleton<IControllersEvents, NullContollerEvents>();

            services.AddScoped<ControllersUpdateHandler>();

            return services;
        }

        private static Assembly GetEntryAssembly()
        {
            var entryAssembly = Assembly.GetEntryAssembly();
            if (entryAssembly == null)
            {
                throw new Exception("Can't find entry assembly. Please specify concrete assemblies for scanning.");
            }
            return entryAssembly;
        }
    }
}
