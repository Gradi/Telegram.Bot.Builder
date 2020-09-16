using System;
using System.Collections.Generic;
using System.Reflection;
using Telegram.Bot.Builder.Controllers.Binders;
using Telegram.Bot.Builder.Controllers.Filters;

namespace Telegram.Bot.Builder.Controllers
{
    /// <summary>
    /// Options for controllers.
    /// </summary>
    public class ControllerOptions
    {
        private readonly List<Type> _filters;

        /// <summary>
        /// Collection of binders.
        /// By default, it starts with binders for standard C# types (strings, char, bool, numbers);
        /// </summary>
        public IBinderCollection Binders { get; }

        /// <summary>
        /// Collection of registered filters.
        /// </summary>
        public IReadOnlyCollection<Type> Filters => _filters;

        /// <summary>
        /// Collection of assemblies that contain controllers.
        /// By default this collection starts with entry assembly.
        /// </summary>
        public ICollection<Assembly> ControllerAssemblies { get; }

        public ControllerOptions()
        {
            _filters = new List<Type>();

            Binders = new BinderCollection();
            ControllerAssemblies = new List<Assembly>();

            var entryAsm = Assembly.GetEntryAssembly();
            if (entryAsm != null)
                ControllerAssemblies.Add(entryAsm);

            AddDefaultBinders();
        }

        /// <summary>
        /// Adds filter to global list of filters.
        /// Dependency injection will be used to retrieve instance.
        /// </summary>
        public ControllerOptions AddFilter<T>() where T : class, IFilter
        {
            _filters.Add(typeof(T));
            return this;
        }

        private void AddDefaultBinders()
        {
            Binders.Add(new BoolBinder());
            Binders.Add(new CharBinder());
            Binders.Add(new StringBinder());
            Binders.Add(new DateTimeBinder());
            Binders.Add(new DateTimeOffsetBinder());
            Binders.Add(new TimeSpanBinder());

            Binders.Add(new ByteBinder());
            Binders.Add(new SByteBinder());
            Binders.Add(new ShortBinder());
            Binders.Add(new UshortBinder());
            Binders.Add(new IntBinder());
            Binders.Add(new UintBinder());
            Binders.Add(new LongBinder());
            Binders.Add(new UlongBinder());
            Binders.Add(new FloatBinder());
            Binders.Add(new DoubleBinder());
            Binders.Add(new DecimalBinder());
        }
    }
}
