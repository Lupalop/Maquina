using Maquina.Elements;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Maquina
{
    public class ElementDictionary : IDictionary<string, BaseElement>
    {
        // Fields
        private IDictionary<string, BaseElement> InnerDictionary;
        private bool _isModified;

        // Constructors
        public ElementDictionary()
        {
            InnerDictionary = new Dictionary<string, BaseElement>();
        }
        public ElementDictionary(IDictionary<string, BaseElement> dictionary)
        {
            InnerDictionary = dictionary;
        }
        public ElementDictionary(IEqualityComparer<string> comparer)
        {
            InnerDictionary = new Dictionary<string, BaseElement>(comparer);
        }
        public ElementDictionary(int capacity)
        {
            InnerDictionary = new Dictionary<string, BaseElement>(capacity);
        }
        public ElementDictionary(IDictionary<string, BaseElement> dictionary, IEqualityComparer<string> comparer)
        {
            InnerDictionary = new Dictionary<string, BaseElement>(dictionary, comparer);
        }
        public ElementDictionary(int capacity, IEqualityComparer<string> comparer)
        {
            InnerDictionary = new Dictionary<string, BaseElement>(capacity, comparer);
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

        public bool TryGetValue(string key, out BaseElement value)
        {
            return InnerDictionary.TryGetValue(key, out value);
        }

        public ICollection<BaseElement> Values
        {
            get { return InnerDictionary.Values; }
        }

        public BaseElement this[string key]
        {
            get { return InnerDictionary[key]; }
            set
            {
                _isModified = true;
                // Stop listening to old element changes and listen to new element
                InnerDictionary[key].ElementChanged -= Child_ElementChanged;
                value.ElementChanged += Child_ElementChanged;
                // Replace old element reference
                InnerDictionary[key] = value;
                // An element was replaced, notify handler
                OnElementChanged(this, new ElementChangedEventArgs(ElementChangedProperty.Size));
            }
        }

        public void Add(string key, BaseElement value)
        {
            Add(new KeyValuePair<string, BaseElement>(key, value));
        }
        public void Add(KeyValuePair<string, BaseElement> item)
        {
            _isModified = true;
            InnerDictionary.Add(item);
            item.Value.ElementChanged += Child_ElementChanged;
            OnElementChanged(this, new ElementChangedEventArgs(ElementChangedProperty.Size));
        }

        public bool Contains(string key, BaseElement value)
        {
            return Contains(new KeyValuePair<string, BaseElement>(key, value));
        }
        public bool Contains(KeyValuePair<string, BaseElement> item)
        {
            return InnerDictionary.Contains(item);
        }

        public void CopyTo(KeyValuePair<string, BaseElement>[] array, int arrayIndex)
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
            _isModified = true;
            InnerDictionary[key].ElementChanged -= Child_ElementChanged;
            bool result = InnerDictionary.Remove(key);
            OnElementChanged(this, new ElementChangedEventArgs(ElementChangedProperty.Size));
            return result;
        }

        public bool Remove(KeyValuePair<string, BaseElement> item)
        {
            _isModified = true;
            InnerDictionary[item.Key].ElementChanged -= Child_ElementChanged;
            bool result = InnerDictionary.Remove(item);
            OnElementChanged(this, new ElementChangedEventArgs(ElementChangedProperty.Size));
            return result;
        }

        public void Clear()
        {
            Clear(false);
        }

        public void Clear(bool disposeElements)
        {
            _isModified = true;
            lock (InnerDictionary)
            {
                foreach (var item in InnerDictionary.Values)
                {
                    item.ElementChanged -= Child_ElementChanged;
                    if (disposeElements)
                    {
                        item.Dispose();
                    }
                }
                InnerDictionary.Clear();
            }
        }

        public IEnumerator<KeyValuePair<string, BaseElement>> GetEnumerator()
        {
            return InnerDictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return InnerDictionary.GetEnumerator();
        }

        public event ElementChangedEventHandler ElementChanged;

        private void OnElementChanged(object sender, ElementChangedEventArgs e)
        {
            if (ElementChanged != null)
            {
                ElementChanged(sender, e);
            }
        }

        private void Child_ElementChanged(object sender, ElementChangedEventArgs e)
        {
            OnElementChanged(sender, e);
        }

        public void Update()
        {
            foreach (var item in Values)
            {
                item.Update();
                if (_isModified)
                {
                    break;
                }
            }
            _isModified = false;
        }

        public void Draw()
        {
            foreach (var item in Values)
            {
                item.Draw();
                if (_isModified)
                {
                    break;
                }
            }
            _isModified = false;
        }
    }
}
