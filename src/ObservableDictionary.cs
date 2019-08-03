using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Maquina
{
    public class ObservableDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        // Fields
        private IDictionary<TKey, TValue> InnerDictionary;

        // Constructors
        public ObservableDictionary()
        {
            InnerDictionary = new Dictionary<TKey,TValue>();
        }
        public ObservableDictionary(IDictionary<TKey, TValue> dictionary)
        {
            InnerDictionary = dictionary;
        }
        public ObservableDictionary(IEqualityComparer<TKey> comparer)
        {
            InnerDictionary = new Dictionary<TKey, TValue>(comparer);
        }
        public ObservableDictionary(int capacity)
        {
            InnerDictionary = new Dictionary<TKey, TValue>(capacity);
        }
        public ObservableDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer)
        {
            InnerDictionary = new Dictionary<TKey, TValue>(dictionary, comparer);
        }
        public ObservableDictionary(int capacity, IEqualityComparer<TKey> comparer)
        {
            InnerDictionary = new Dictionary<TKey, TValue>(capacity, comparer);
        }

        // Implemented methods
        public bool ContainsKey(TKey key)
        {
            return InnerDictionary.ContainsKey(key);
        }

        public ICollection<TKey> Keys
        {
            get { return InnerDictionary.Keys; }
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return InnerDictionary.TryGetValue(key, out value);
        }

        public ICollection<TValue> Values
        {
            get { return InnerDictionary.Values; }
        }

        public TValue this[TKey key]
        {
            get { return InnerDictionary[key]; }
            set
            {
                InnerDictionary[key] = value;
                OnCollectionChanged(NotifyCollectionChangedAction.Replace, InnerDictionary[key], value);
            }
        }

        public void Add(TKey key, TValue value)
        {
            this.Add(new KeyValuePair<TKey, TValue>(key, value));
        }
        public void Add(KeyValuePair<TKey, TValue> item)
        {
            if (ContainsKey(item.Key))
            {
#if LOG_ENABLED
                LogManager.Error(0, "An item with the same key has already been added.");
#endif
                return;
            }

            InnerDictionary.Add(item);
            OnCollectionChanged(NotifyCollectionChangedAction.Add, item.Value);
        }

        public bool Contains(TKey key, TValue value)
        {
            return this.Contains(new KeyValuePair<TKey, TValue>(key, value));
        }
        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return InnerDictionary.Contains(item);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            InnerDictionary.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return InnerDictionary.Count; }
        }

        public bool IsReadOnly
        {
            get { return InnerDictionary.IsReadOnly; }
        }

        public bool Remove(TKey key)
        {
            if (!ContainsKey(key))
            {
#if LOG_ENABLED
                LogManager.Error(0, string.Format("Attempting to remove a non-existent item: {0}", key));
#endif
                return false;
            }
            object removedObject = InnerDictionary[key];
            bool result = InnerDictionary.Remove(key);
            OnCollectionChanged(NotifyCollectionChangedAction.Remove, removedObject);
            return result;
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            if (!Contains(item))
            {
#if HAS_CONSOLE
                LogManager.Error(0, string.Format("Attempting to remove a non-existent item: {0}", item.Key));
#endif
                return false;
            }
            object removedObject = InnerDictionary[item.Key];
            bool result = InnerDictionary.Remove(item);
            OnCollectionChanged(NotifyCollectionChangedAction.Remove, removedObject);
            return result;
        }

        public void Clear()
        {
            InnerDictionary.Clear();
            OnCollectionReset();
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return InnerDictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return InnerDictionary.GetEnumerator();
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (CollectionChanged != null)
            {
                CollectionChanged(this, e);
            }
        }

        private void OnCollectionChanged(NotifyCollectionChangedAction action, object item)
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, item));
        }

        private void OnCollectionChanged(NotifyCollectionChangedAction action, object oldItem, object newItem)
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, oldItem, newItem));
        }

        private void OnCollectionReset()
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
    }
}
