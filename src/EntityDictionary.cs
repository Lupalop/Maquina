using Maquina.Entities;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Maquina
{
    public class EntityDictionary : IDictionary<string, Entity>
    {
        // Fields
        private IDictionary<string, Entity> InnerDictionary;
        public bool IsModified { get; protected set; }

        // Constructors
        public EntityDictionary()
        {
            InnerDictionary = new Dictionary<string, Entity>();
        }
        public EntityDictionary(IDictionary<string, Entity> dictionary)
        {
            InnerDictionary = dictionary;
        }
        public EntityDictionary(IEqualityComparer<string> comparer)
        {
            InnerDictionary = new Dictionary<string, Entity>(comparer);
        }
        public EntityDictionary(int capacity)
        {
            InnerDictionary = new Dictionary<string, Entity>(capacity);
        }
        public EntityDictionary(IDictionary<string, Entity> dictionary, IEqualityComparer<string> comparer)
        {
            InnerDictionary = new Dictionary<string, Entity>(dictionary, comparer);
        }
        public EntityDictionary(int capacity, IEqualityComparer<string> comparer)
        {
            InnerDictionary = new Dictionary<string, Entity>(capacity, comparer);
        }

        // Implemented methods
        public bool ContainsKey(string key)
        {
            return InnerDictionary.ContainsKey(key);
        }

        public ICollection<string> Keys
        {
            get { return InnerDictionary.Keys; }
        }

        public bool TryGetValue(string key, out Entity value)
        {
            return InnerDictionary.TryGetValue(key, out value);
        }

        public ICollection<Entity> Values
        {
            get { return InnerDictionary.Values; }
        }

        public Entity this[string key]
        {
            get { return InnerDictionary[key]; }
            set
            {
                IsModified = true;
                // Stop listening to old entity changes and listen to new entity
                InnerDictionary[key].Changed -= Child_EntityChanged;
                value.Changed += Child_EntityChanged;
                // Replace old entity reference
                InnerDictionary[key] = value;
                // An entity was replaced, notify handler
                OnEntityChanged(this, new EntityChangedEventArgs(EntityChangedProperty.Size));
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
            InnerDictionary.Add(item);
            item.Value.Changed += Child_EntityChanged;
            OnEntityChanged(this, new EntityChangedEventArgs(EntityChangedProperty.Size));
        }

        public bool Contains(string key, Entity value)
        {
            return Contains(new KeyValuePair<string, Entity>(key, value));
        }
        public bool Contains(KeyValuePair<string, Entity> item)
        {
            return InnerDictionary.Contains(item);
        }

        public void CopyTo(KeyValuePair<string, Entity>[] array, int arrayIndex)
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

        public bool Remove(string key)
        {
            IsModified = true;
            InnerDictionary[key].Changed -= Child_EntityChanged;
            bool result = InnerDictionary.Remove(key);
            OnEntityChanged(this, new EntityChangedEventArgs(EntityChangedProperty.Size));
            return result;
        }

        public bool Remove(KeyValuePair<string, Entity> item)
        {
            IsModified = true;
            InnerDictionary[item.Key].Changed -= Child_EntityChanged;
            bool result = InnerDictionary.Remove(item);
            OnEntityChanged(this, new EntityChangedEventArgs(EntityChangedProperty.Size));
            return result;
        }

        public void Clear()
        {
            Clear(false);
        }

        public void Clear(bool disposeEntities)
        {
            IsModified = true;
            lock (InnerDictionary)
            {
                foreach (var item in InnerDictionary.Values)
                {
                    item.Changed -= Child_EntityChanged;
                    if (disposeEntities)
                    {
                        item.Dispose();
                    }
                }
                InnerDictionary.Clear();
            }
        }

        public IEnumerator<KeyValuePair<string, Entity>> GetEnumerator()
        {
            return InnerDictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return InnerDictionary.GetEnumerator();
        }

        public event EventHandler<EntityChangedEventArgs> EntityChanged;

        protected virtual void OnEntityChanged(object sender, EntityChangedEventArgs e)
        {
            if (EntityChanged != null)
            {
                EntityChanged(sender, e);
            }
        }

        private void Child_EntityChanged(object sender, EntityChangedEventArgs e)
        {
            OnEntityChanged(sender, e);
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
    }
}
