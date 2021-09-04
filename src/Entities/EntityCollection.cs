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
        private Entity _parent;

        public EntityCollection(Entity parent)
        {
            _parent = parent;
        }

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
            Entity oldItem = Items[index];
            oldItem.PropertyChanged -= OnEntityChanged;
            oldItem.Parent = null;
            item.PropertyChanged += OnEntityChanged;
            item.Parent = _parent;
            base.SetItem(index, item);
        }

        protected override void InsertItem(int index, Entity item)
        {
            item.PropertyChanged += OnEntityChanged;
            item.Parent = _parent;
            base.InsertItem(index, item);
        }

        protected override void RemoveItem(int index)
        {
            Entity item = Items[index];
            item.PropertyChanged -= OnEntityChanged;
            item.Parent = null;
            base.RemoveItem(index);
        }

        protected override void ClearItems()
        {
            for (int i = 0; i < Count; i++)
            {
                Entity item = Items[i];
                item.PropertyChanged -= OnEntityChanged;
                item.Parent = null;
            }
            base.ClearItems();
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
            for (int i = 0; i < Count; i++)
            {
                Items[i].Update();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < Count; i++)
            {
                Items[i].Draw(spriteBatch);
            }
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
