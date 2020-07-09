using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Specialized;

namespace Maquina.Entities
{
    public class EntityCollection : ObservableCollection<Entity>, IDisposable
    {
        public bool IsModified { get; private set; }

        private bool IsIndexValid(int index)
        {
            return (index >= 0 && index < Count);
        }

        public virtual Entity this[string key]
        {
            get
            {
                if (string.IsNullOrEmpty(key))
                {
                    return null;
                }
                int index = IndexOfKey(key);
                if (IsIndexValid(index))
                {
                    return Items[index];
                }
                return null;
            }
        }

        public virtual int IndexOfKey(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return -1;
            }

            for (int i = 0; i < Count; i++)
            {
                if (this[i].Name.ToLowerInvariant() == key.ToLowerInvariant())
                {
                    return i;
                }
            }

            return -1;
        }

        public bool Remove(string key)
        {
            Entity entity = this[key];
            return Remove(entity);
        }

        protected override void SetItem(int index, Entity item)
        {
            Items[index].PropertyChanged -= OnEntityChanged;
            item.PropertyChanged += OnEntityChanged;
            base.SetItem(index, item);
        }

        protected override void InsertItem(int index, Entity item)
        {
            item.PropertyChanged += OnEntityChanged;
            base.InsertItem(index, item);
        }

        protected override void RemoveItem(int index)
        {
            Items[index].PropertyChanged -= OnEntityChanged;
            base.RemoveItem(index);
        }

        protected override void ClearItems()
        {
            for (int i = 0; i < Count; i++)
            {
                Items[i].PropertyChanged -= OnEntityChanged;
            }
            base.ClearItems();
        }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            IsModified = true;
            base.OnCollectionChanged(e);
        }

        public bool ContainsKey(string key)
        {
            return IsIndexValid(IndexOfKey(key));
        }

        public event EventHandler<PropertyChangedEventArgs> EntityChanged;

        protected virtual void OnEntityChanged(object sender, PropertyChangedEventArgs e)
        {
            if (EntityChanged != null)
            {
                EntityChanged(sender, e);
            }
        }

        public void Update()
        {
            foreach (Entity entity in Items)
            {
                entity.Update();
                if (IsModified)
                {
                    break;
                }
            }
            IsModified = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Entity entity in Items)
            {
                entity.Draw(spriteBatch);
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
                for (int i = 0; i < Count; i++)
                {
                    Items[i].Dispose();
                }
                base.ClearItems();
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
