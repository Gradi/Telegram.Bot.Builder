using System;
using System.Collections;
using System.Collections.Generic;

namespace Telegram.Bot.Builder.Controllers.Binders
{
    public class BinderCollection : IBinderCollection
    {
        private readonly List<IBinder> _binders;

        public int Count => _binders.Count;

        public bool IsReadOnly => false;

        public BinderCollection()
        {
            _binders = new List<IBinder>();
        }

        public IEnumerator<IBinder> GetEnumerator() => _binders.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void Add(IBinder item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            _binders.Insert(0, item);
        }

        public void Clear() => _binders.Clear();

        public bool Contains(IBinder item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            return _binders.Contains(item);
        }

        public void CopyTo(IBinder[] array, int arrayIndex) => _binders.CopyTo(array, arrayIndex);

        public bool Remove(IBinder item) => item != null ? _binders.Remove(item) : false;
    }
}
