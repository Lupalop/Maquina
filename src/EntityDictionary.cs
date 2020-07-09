using Maquina.Entities;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Maquina
{
    public class EntityDictionary : IDictionary<string, Entity>, IDisposable, INotifyCollectionChanged
    {
        private IDictionary<string, Entity> _innerDictionary;

        public EntityDictionary()
        {
            _innerDictionary = new Dictionary<string, Entity>();
        }

        public EntityDictionary(IDictionary<string, Entity> dictionary)
        {
            _innerDictionary = dictionary;
        }

        public EntityDictionary(IEqualityComparer<string> comparer)
        {
            _innerDictionary = new Dictionary<string, Entity>(comparer);
        }

        public EntityDictionary(int capacity)
        {
            _innerDictionary = new Dictionary<string, Entity>(capacity);
        }

        public EntityDictionary(IDictionary<string, Entity> dictionary, IEqualityComparer<string> comparer)
        {
            _innerDictionary = new Dictionary<string, Entity>(dictionary, comparer);
        }

        public EntityDictionary(int capacity, IEqualityComparer<string> comparer)
        {
            _innerDictionary = new Dictionary<string, Entity>(capacity, comparer);
        }

        public bool ContainsKey(string key)
        {
            return _innerDictionary.ContainsKey(key);
        }

        public ICollection<string> Keys
        {
            get { return _innerDictionary.Keys; }
        }

        public bool TryGetValue(string key, out Entity value)
        {
            return _innerDictionary.TryGetValue(key, out value);
        }

        public ICollection<Entity> Values
        {
            get { return _innerDictionary.Values; }
        }

        public bool IsModified { get; set; }

        public Entity this[string key]
        {
            get { return _innerDictionary[key]; }
            set
            {
                IsModified = true;
                _innerDictionary[key].Changed -= OnEntityChanged;
                value.Changed += OnEntityChanged;
                _innerDictionary[key] = value;
                OnCollectionChanged(
                    this,
                    new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace));
            }
        }

        public void Add(Entity value)
        {
            Add(new KeyValuePair<string, Entity>(value.Name, value));
        }
        public void Add(string key, Entity value)
        {
            Add(new KeyValuePair<string, Entity>(key, value));
        }
        public void Add(KeyValuePair<string, Entity> item)
        {
            IsModified = true;
            _innerDictionary.Add(item);
            item.Value.Changed += OnEntityChanged;
            OnCollectionChanged(
                this,
                new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add));

        }

        public bool Contains(string key, Entity value)
        {
            return Contains(new KeyValuePair<string, Entity>(key, value));
        }
        public bool Contains(KeyValuePair<string, Entity> item)
        {
            return _innerDictionary.Contains(item);
        }

        public void CopyTo(KeyValuePair<string, Entity>[] array, int arrayIndex)
        {
            _innerDictionary.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return _innerDictionary.Count; }
        }

        public bool IsReadOnly
        {
            get { return _innerDictionary.IsReadOnly; }
        }

        public bool Remove(string key)
        {
            IsModified = true;
            _innerDictionary[key].Changed -= OnEntityChanged;
            bool result = _innerDictionary.Remove(key);
            OnCollectionChanged(
                this,
                new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove));
            return result;
        }

        public bool Remove(KeyValuePair<string, Entity> item)
        {
            IsModified = true;
            _innerDictionary[item.Key].Changed -= OnEntityChanged;
            bool result = _innerDictionary.Remove(item);
            OnCollectionChanged(
                this,
                new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove));
            return result;
        }

        protected void Clear(bool disposeElements)
        {
            IsModified = true;
            lock (_innerDictionary)
            {
                foreach (var item in _innerDictionary.Values)
                {
                    item.Changed -= OnEntityChanged;
                    if (disposeElements)
                    {
                        item.Dispose();
                    }
                }
                _innerDictionary.Clear();
            }
            OnCollectionChanged(
                this,
                new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public void Clear()
        {
            Clear(false);
        }

        public IEnumerator<KeyValuePair<string, Entity>> GetEnumerator()
        {
            return _innerDictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _innerDictionary.GetEnumerator();
        }

        public event EventHandler<EntityChangedEventArgs> EntityChanged;
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        protected virtual void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (CollectionChanged != null)
            {
                CollectionChanged(sender, e);
            }
        }

        protected virtual void OnEntityChanged(object sender, EntityChangedEventArgs e)
        {
            if (EntityChanged != null)
            {
                EntityChanged(sender, e);
            }
        }

        public void Update()
        {
            foreach (var item in Values)
            {
                item.Update();
                if (IsModified)
                {
                    break;
                }
            }
            IsModified = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var item in Values)
            {
                item.Draw(spriteBatch);
                if (IsModified)
                {
                    break;
                }
            }
            IsModified = false;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Clear(true);
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
