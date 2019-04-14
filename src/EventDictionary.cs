using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Maquina
{
    [Serializable]
    public class EventDictionary<TKey, TValue> : Dictionary<TKey, TValue>
    {
        public EventDictionary() : base() { }
        public EventDictionary(IDictionary<TKey, TValue> dictionary) : base(dictionary) { }
        public EventDictionary(IEqualityComparer<TKey> comparer) : base(comparer) { }
        public EventDictionary(int capacity) : base(capacity) { }
        public EventDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer) : base(dictionary, comparer) { }
        public EventDictionary(int capacity, IEqualityComparer<TKey> comparer) : base(capacity, comparer) { }
        protected EventDictionary(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public event Action<TKey, TValue> ItemAdded;
        public event Action<TKey, TValue> ItemRemoved;
        public event Action DictionaryCleared;

        public new void Add(TKey key, TValue value)
        {
            if (ItemAdded != null)
            {
                ItemAdded(key, value);
            }
            base.Add(key, value);
        }

        public new bool Remove(TKey key)
        {
            if (ItemRemoved != null)
            {
                ItemRemoved(key, base[key]);
            }
            return base.Remove(key);
        }

        public new void Clear()
        {
            if (DictionaryCleared != null)
            {
                DictionaryCleared();
            }
            base.Clear();
        }
    }
}
