using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Maquina
{
    [Serializable]
    public class ObservableDictionary<TKey, TValue> : Dictionary<TKey, TValue>
    {
        public ObservableDictionary() : base() { }
        public ObservableDictionary(IDictionary<TKey, TValue> dictionary) : base(dictionary) { }
        public ObservableDictionary(IEqualityComparer<TKey> comparer) : base(comparer) { }
        public ObservableDictionary(int capacity) : base(capacity) { }
        public ObservableDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer) : base(dictionary, comparer) { }
        public ObservableDictionary(int capacity, IEqualityComparer<TKey> comparer) : base(capacity, comparer) { }
        protected ObservableDictionary(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public event Action<TKey, TValue> ItemAdded;
        public event Action<TKey, TValue> ItemRemoved;
        public event Action DictionaryCleared;
        public event Action CollectionModified;

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

        public new void Add(TKey key, TValue value)
        {
            if (ContainsKey(key))
            {
#if HAS_CONSOLE
                Console.WriteLine("An item with the same key has already been added.");
#endif
                return;
            }
            if (ItemAdded != null)
            {
                ItemAdded(key, value);
            }
            if (CollectionModified != null)
            {
                CollectionModified();
            }
            base.Add(key, value);
        }

        public new bool Remove(TKey key)
        {
            if (!ContainsKey(key))
            {
#if HAS_CONSOLE
                Console.WriteLine(String.Format("Attempting to remove a non-existent item: {0}", key));
#endif
                return false;
            }
            if (ItemRemoved != null)
            {
                ItemRemoved(key, base[key]);
            }
            if (CollectionModified != null)
            {
                CollectionModified();
            }
            return base.Remove(key);
        }

        public new void Clear()
        {
            if (DictionaryCleared != null)
            {
                DictionaryCleared();
            }
            if (CollectionModified != null)
            {
                CollectionModified();
            }
            base.Clear();
        }
    }
}
